using System;

namespace Sexp {
    public abstract class DatumVisitor {
        public virtual void visit() { }
        public virtual void visitEnd() { }
    }

    public class VectorVisitor : DatumVisitor {
        public virtual AtomVisitor visitItem_Atom() { return null; }
        public virtual ConsVisitor visitItem_Cons() { return null; }
        public virtual VectorVisitor visitItem_Vector() { return null; }
        public virtual void visitItem(object o) { }
    }

    public class AtomVisitor : DatumVisitor {
        public virtual void visit_value(Boolean o) { }
        public virtual void visit_value(Int64 o) { }
        public virtual void visit_value(Double o) { }
        public virtual void visit_value(String o) { }
        public virtual void visit_value(Char o) { }
        public virtual void visit_value(Object o) { }
        public virtual void visit_value(Symbol o) { }
    }

    public class ConsVisitor : DatumVisitor {
//        public virtual KeywordVisitor visit_Keyword_car() { return null; }
        public virtual AtomVisitor visit_Atom_car() { return null; }
        public virtual ConsVisitor visit_Cons_car() { return null; }
        public virtual VectorVisitor visit_Vector_car() { return null; }
        public virtual AtomVisitor visit_Atom_cdr() { return null; }
        public virtual ConsVisitor visit_Cons_cdr() { return null; }
        public virtual VectorVisitor visit_Vector_cdr() { return null; }
        public virtual void visit_car(object o) { }
        public virtual void visit_cdr(object o) { }
    }
}