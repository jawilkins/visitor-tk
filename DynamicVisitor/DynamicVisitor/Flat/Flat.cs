using System;
using System.Collections.Generic;

using ConstructLang;
using GuidedTour;

using Symbols;

// I wanted to call the intermediate language「つるぺた」
// that means "flat and smooth," but I reconsidered...
// That has a less literal meaning if you care to look it up.
// Instead I decided to call it simply -♭
// Not - C♭
// I thought maybe λ♭- but no...
// Just take Scheme and remove most recursive combinations
// Wouldn't that be pretty flat?
// 5-3-2008

namespace Flat {
    public abstract class ListWrapper<T> : IEnumerable<T> {
        List<T> list = new List<T>();

        protected ListWrapper(params T[] items)
        {
            list.AddRange(items);
        }

        internal void Add(T item)
        {
            list.Add(item);
        }

        internal int Count { get { return list.Count; } }

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return list.GetEnumerator() as IEnumerator<T>;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion
    }

    [RepresentativeField("name")]
    public class Type {
        public Symbol name;

        internal Type(string name)
        {
            this.name = Symbol.get_symbol(name);
        }
    }

    public class Types : ListWrapper<Type> { public Types(params Type[] list) : base(list) { } }

    [SiteList("name", "parameter_types", "result_types")]
    public class Prototype : Type {
        public Types parameter_types;
        public Types result_types;

        internal Prototype(string name, Types parameter_types, Types return_types)
            : base(name)
        {
            this.parameter_types = parameter_types;
            this.result_types = return_types;
        }
    }

    public class Prototypes : ListWrapper<Prototype> { internal Prototypes() { } }

    public abstract class Operand {
        public Type type;

        internal Operand(Type type)
        {
            this.type = type;
        }
    }

    [LiteralField("value"), SiteList("type", "value")]
    public class Literal : Operand {
        public object value;

        internal Literal(Type type, object value)
            : base(type)
        {
            this.value = value;
        }
    }

    public class Literals : ListWrapper<Literal> { }

    [RepresentativeField("name")]
    public abstract class NamedOperand : Operand {
        public Symbol name;

        internal NamedOperand(string name, Type type)
            : base(type)
        {
            this.name = Symbol.get_symbol(name);
        }
    }

    public class Operands : ListWrapper<Operand> { public Operands(params Operand[] list) : base(list) { } }

    [SiteList("name", "type", "value")]
    public class Constant : NamedOperand {
        public object value;

        internal Constant(string name, Type type, object value)
            : base(name, type)
        {
            this.value = value;
        }
    }

    public class Constants : ListWrapper<Constant> { }

    public abstract class Lvalue : NamedOperand {
        internal Lvalue(string name, Type type) : base(name, type) { }
    }

    public class Lvalues : ListWrapper<Lvalue> { public Lvalues(params Lvalue[] list) : base(list) { } }

    public class Global : Lvalue {
        internal Global(string name, Type type) : base(name, type) { }
    }

    public class Globals : ListWrapper<Global> { }

    public class Local : Lvalue, Line {
        internal Local(string name, Type type) : base(name, type) { }
    }

    public class Locals : ListWrapper<Local> { }

    public class Parameter : Lvalue, Line {
        internal Parameter(string name, Type type) : base(name, type) { }
    }

    public class Parameters : ListWrapper<Parameter> { }

    [RepresentativeField("name"), SiteList("result_types", "name", "operand_types")]
    public class Operator {
        public Types result_types;
        public Symbol name;
        public Types operand_types;

        internal Operator(string name, Types result_types, Types operand_types)
        {
            this.name = Symbol.get_symbol(name);
            this.result_types = result_types;
            this.operand_types = operand_types;
        }
    }

    public class Operators : ListWrapper<Operator> { }

    [RepresentativeField("name"), SiteList("result_types", "name", "listing")]
    public class Lambda : Line {
        public Types result_types;
        public Symbol name;
        public Listing listing = new Listing();

        internal Lambda parent;

        Parameters parameters = new Parameters();
        Locals locals = new Locals();
        Lambdas lambdas = new Lambdas();
        Statements statements = new Statements();

        public void addParameter(Parameter param)
        {
            parameters.Add(param);
            listing.Add(param);
        }

        public void addLocal(Local local)
        {
            locals.Add(local);
            listing.Add(local);
        }
        
        public void addLambda(Lambda lambda)
        {
            lambdas.Add(lambda);
            listing.Add(lambda);
        }
        
        public void addStatement(Statement stmt)
        {
            statements.Add(stmt);
            listing.Add(stmt);
        }

        internal Lambda(string name, Types return_types, Lambda parent)
        {
            this.name = Symbol.get_symbol(name);
            this.result_types = return_types;
            this.parent = parent;
        }
    }

    public class Lambdas : ListWrapper<Lambda> { }

    public interface Line { }

    public class Listing : ListWrapper<Line> { }

    public abstract class Statement : Line {
        public Lvalues lvalues;

        internal Statement(Lvalues lvalues)
        {
            this.lvalues = lvalues;
        }
    }

    public class Statements : ListWrapper<Statement> { }

    [SiteList("lvalues", "op", "arguments")]
    public class Do : Statement {
        public Operator op;
        public Operands arguments;

        internal Do(Lvalues lvalues, Operator op, Operands arguments)
            : base(lvalues)
        {
            this.op = op;
            this.arguments = arguments;
        }
    }

    [SiteList("lvalues", "lambda", "arguments")]
    public class DoLambda : Statement {
        Lambda lambda;
        Operands arguments;

        internal DoLambda(Lvalues lvalues, Lambda lambda, Operands arguments)
            : base(lvalues)
        {
            this.lambda = lambda;
            this.arguments = arguments;
        }
    }

    [SiteList("lvalues", "lambda_ref", "arguments")]
    public class Call : Statement {
        public Operand lambda_ref;
        public Operands arguments;

        internal Call(Lvalues lvalues, Operand lambda_reference, Operands arguments)
            : base(lvalues)
        {
            this.lambda_ref = lambda_reference;
            this.arguments = arguments;
        }
    }

    [RepresentativeField("name"), SiteList("name", "element_types")]
    public class Relation {
        public Symbol name;
        public Types element_types;

        internal Relation(string name, Types element_types)
        {
            this.name = Symbol.get_symbol(name);
            this.element_types = element_types;
        }
    }

    public class Relations : ListWrapper<Relation> { }

    [SiteList("conditional", "consequent", "alternate")]
    public class If : Statement {
        public Relation conditional;
        public Lambda consequent;
        public Lambda alternate;

        internal If(Lvalues lvalues, Relation conditional, Lambda consequent, Lambda alternate)
            : base(lvalues)
        {
            this.conditional = conditional;
            this.consequent = consequent;
            this.alternate = alternate;
        }
    }

    [SiteList("lvalues", "rvalues")]
    public class Move : Statement {
        public Operands rvalues;

        internal Move(Lvalues lvalues, Operands rvalues)
            : base(lvalues)
        {
            this.rvalues = rvalues;
        }
    }

    [SiteList("types", "prototypes", "constants", "globals", "operators", "relations", "lambdas")]
    public class Code {
        public Types types = new Types();
        public Prototypes prototypes = new Prototypes();
        public Literals literals = new Literals();
        public Constants constants = new Constants();
        public Globals globals = new Globals();
        public Operators operators = new Operators();
        public Relations relations = new Relations();
        public Lambdas lambdas = new Lambdas();
    }
}