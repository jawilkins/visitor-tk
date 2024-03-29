//#define WARNING
//#define VERBOSE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Util;
using GuidedTour;

namespace DynamicVisitor {
    public class VisitSiteseer : ISiteseer {
        static readonly Type[] no_params = { };
        static readonly object[] no_args = { };

        Object m_visitor;
        Type m_visitor_type;

        Dictionary<string, string> m_aliases;

        public VisitSiteseer(Object visitor)
        {
            init(visitor);
            m_aliases = new Dictionary<string, string>();
        }

        VisitSiteseer(Object visitor, Dictionary<string, string> aliases)
        {
            if (null == aliases) throw new ArgumentNullException("aliases");

            init(visitor);
            m_aliases = aliases;
        }

        void init(Object visitor)
        {
            if (null == visitor) throw new ArgumentNullException("visitor");

            m_visitor = visitor;
            m_visitor_type = visitor.GetType();
        }

        public void AddAlias(string typename, string tag)
        {
            m_aliases.Add(typename, tag);
        }

        public bool view_whole(Site node)
        {
            Type type = node.type;

            while (type != null) {
                Type[] param = { type };
                MethodInfo mi = GetMethod("visit", param);

                if (mi != null) {
                    Object[] arg = { node.value };
                    mi.Invoke(m_visitor, arg);
                    return true;
                } else {
                    Trace.Verbose("debug: could not find method {0}.visit({1}), skipping", m_visitor_type.FullName, node.type.FullName);
                }

                type = type.BaseType;
            }

            return false;
        }

        public void begin()
        {
            MethodInfo mi = GetMethod("visit", no_params);

            if (mi != null) {
                mi.Invoke(m_visitor, no_args);
            } else {
                Trace.Verbose("debug: could not find method {0}.visit(), skipping", m_visitor_type.FullName);
            }
        }

        public void end()
        {
            MethodInfo mi = GetMethod("visitEnd", no_params);

            if (mi != null) {
                mi.Invoke(m_visitor, no_args);
            } else {
                Trace.Verbose("debug: could not find method {0}.visitEnd(), skipping", m_visitor_type.FullName);
            }
        }

        public bool view_part(Site node, out ISiteseer new_op)
        {
            string prefix;

            if (node.name != null) {
                prefix = "visit";
            } else {
                prefix = "visitItem";
            }

            return
                invokeNonTerminal(prefix, node.type, node.name, out new_op) ||
                invokeTerminal(prefix+(node.name!=null?"_"+node.name:""), node.type, node.value, out new_op) ||
                invokeInline(prefix, node.type, node.name, node.value, out new_op);
        }

        MethodInfo GetMethod(string name, Type[] param)
        {
            return m_visitor_type.GetMethod(
                name,
                BindingFlags.Public|BindingFlags.Instance|BindingFlags.ExactBinding,
                null,
                param,
                null);
        }

        string make_type_string(params Type[] types)
        {
            StringBuilder sb = new StringBuilder();
            bool is_first = true;

            foreach (Type t in types) {
                if (is_first) {
                    is_first = false;
                } else {
                    sb.Append(", ");
                }

                sb.Append(t.Name);
            }

            return sb.ToString();
        }

        bool invokeInline(string prefix, Type type, string name, object value, out ISiteseer new_op)
        {
            if (value != null) {
                MemberInfo[] graph_props = type.FindMembers(MemberTypes.Property|MemberTypes.Field, BindingFlags.Public|BindingFlags.Instance, null, null);
                List<Type> param_list = new List<Type>();
                List<Object> args_list = new List<Object>();

                foreach (MemberInfo p in graph_props) {
                    if (MemberTypes.Field == p.MemberType) {
                        FieldInfo fi = type.GetField(p.Name);
                        args_list.Add(fi.GetValue(value));
                        param_list.Add(fi.FieldType);
                    } else {
                        PropertyInfo pi = type.GetProperty(p.Name);
                        if (0 == pi.GetIndexParameters().Length) {
                            args_list.Add(pi.GetValue(value, null));
                            param_list.Add(pi.PropertyType);
                        }
                    }
                }

                Type[] param_array = new Type[param_list.Count];
                Object[] arg_array = new Object[args_list.Count];

                param_list.CopyTo(param_array);
                args_list.CopyTo(arg_array);

                string method_name = prefix+"_"+type.Name+(name!=null?"_"+name:"");
                MethodInfo mi = GetMethod(method_name, param_array);

                if (mi != null) {
                    Object new_visitor = mi.Invoke(m_visitor, arg_array);

                    if (null == new_visitor) {
                        new_op = null;
                        Trace.Warning("debug: {0}.{1}({2}) did not return a new visitor", m_visitor_type.FullName, method_name, make_type_string(param_array));
                    } else {
                        new_op = new VisitSiteseer(new_visitor, m_aliases);
                    }

                    return true;
                } else {
                    Trace.Warning("debug: could not find method {0}.{1}({2}), skipping", m_visitor_type.FullName, method_name, make_type_string(param_array));
                }
            }

            new_op = null;

            return false;
        }

        bool invokeTerminal(string method_name, Type type, object value, out ISiteseer new_op)
        {
            while (type != null) {
                Type[] types = { type };
                MethodInfo mi = GetMethod(method_name, types);

                if (mi != null) {
                    object[] args = { value };
                    Object new_visitor = mi.Invoke(m_visitor, args);

                    if (null == new_visitor) {
                        new_op = null;
                        Trace.Warning("debug: {0}.{1}({2}) did not return a new visitor", m_visitor_type.FullName, method_name, type.FullName);
                    } else {
                        new_op = new VisitSiteseer(new_visitor, m_aliases);
                    }

                    return true;
                } else {
                    Trace.Warning("debug: could not find method {0}.{1}({2}), skipping", m_visitor_type.FullName, method_name, type.FullName);
                    type = type.BaseType;
                }
            }

            new_op = null;

            return false;
        }

        bool innerCall(string method_name, out ISiteseer new_method, bool is_last)
        {
            MethodInfo mi = GetMethod(method_name, no_params);

            if (mi != null) {
                Object new_visitor = mi.Invoke(m_visitor, no_args);

                if (null == new_visitor) {
                    new_method = null;
                    if (!is_last) Trace.Verbose("debug: {0} did not return a new visitor", method_name);
                } else {
                    new_method = new VisitSiteseer(new_visitor, m_aliases);
                }

                return true;
            } else {
                if (is_last) Trace.Warning("debug: could not find method {0}.{1}(), skipping", m_visitor_type.FullName, method_name);
                new_method = null;
                return false;
            }
        }

        string getMethodTag(Type type)
        {
            string alias_tag;

            if (m_aliases.TryGetValue(type.FullName, out alias_tag)) {
                return alias_tag;
            } else {
                return type.HasElementType || type.IsGenericType ? null : type.Name;
            }
        }

        bool invokeNonTerminal(string prefix, Type type, string name, out ISiteseer new_method)
        {
            while (type != null) {
                string type_name = getMethodTag(type);

                if (type_name != null) {
                    if (innerCall(prefix+"_"+type_name+(name!=null?"_"+name:""), out new_method, false)) {
                        return true;
                    }
                }

                type = type.BaseType;
            }

            return innerCall(prefix+(name!=null?"_"+name:""), out new_method, true);
        }
    }
}
