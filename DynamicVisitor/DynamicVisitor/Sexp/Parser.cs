using System;
using System.Text;

using Util;

namespace Sexp {
    public class Parser {
        Scanner m_scanner;
        Attributes m_attrib;
        VectVisitor m_visitor;
        TxtLocation m_loc;

        TxtLocation m_error_loc = new TxtLocation();

        int m_errors = 0;
        public int errors { get { return m_errors; } }

        public Parser(Reader reader, VectVisitor visitor)
            : this(reader, visitor, null)
        { }

        public Parser(Reader reader, VectVisitor visitor, TxtLocation loc)
        {
            m_scanner = new Scanner(reader);
            m_visitor = visitor;
            m_loc = loc;
        }

        public void read()
        {
            start_read();
        }

        Token lookahead
        {
            get { return m_attrib.token; }
        }

        Token[] pack(params Token[] tokens)
        {
            return tokens;
        }

        string fancy_token(Token t)
        {
            return '<' + Enum.GetName(t.GetType(), t).ToLower().Replace('_', '-') + '>';
        }

        string token_string(Token[] tokens)
        {
            StringBuilder sb = new StringBuilder();
            bool is_first = true;

            foreach (Token t in tokens) {
                if (is_first) {
                    is_first = false;
                } else {
                    sb.Append(", ");
                }

                sb.Append(fancy_token(t));
            }

            return sb.ToString();
        }

        void expecting(string context, Token[] tokens)
        {
            if (0 == m_errors || m_error_loc.line != m_attrib.loc.line || m_error_loc.column != m_attrib.loc.column) {
                m_error_loc.copy(m_attrib.loc);
                
                Console.WriteLine(
                    "{0}: {1}: expected {2} got {3}",
                    m_attrib.loc.ToString(),
                    context,
                    token_string(tokens),
                    fancy_token(lookahead));

                Console.WriteLine();
                Console.WriteLine(m_attrib.loc.context);
                Console.WriteLine("^".PadLeft(m_attrib.loc.column, '_'));
                Console.WriteLine();

                m_errors++;
            }
        }

        void next()
        {
            m_attrib = m_scanner.scan();

            if (m_loc != null) m_loc.copy(m_attrib.loc);
        }

        void match(Token tok)
        {
            if (lookahead == tok) {
                next();
            } else {
                expecting("match", pack(tok));
            }
        }

        void start_read()
        {
            next();
            m_visitor.visit();
            top_level(m_visitor);
            m_visitor.visitEnd();
            match(Token.EOF);
        }

        void top_level(VectVisitor vec)
        {
            datum_list(vec);

            if (Token.EOF != lookahead) {
                // ERROR
                expecting("top_level", pack(Token.EOF));
            }
        }

        void datum_list(VectVisitor vec)
        {
            while (
                Token.BOOL == lookahead ||
                Token.NUM == lookahead ||
                Token.CHAR == lookahead ||
                Token.STRING == lookahead ||
                Token.ID == lookahead ||
                Token.OPEN_PAREN == lookahead ||
                Token.SINGLE_QUOTE == lookahead ||
                Token.BACKQUOTE == lookahead ||
                Token.COMMA == lookahead ||
                Token.SPLICE == lookahead ||
                Token.VECTOR == lookahead) {

                top_datum(vec);
            }
        }

        void top_datum(VectVisitor top)
        {
            if (Token.BOOL == lookahead ||
                Token.NUM == lookahead ||
                Token.CHAR == lookahead ||
                Token.STRING == lookahead ||
                Token.ID == lookahead) {

                AtomVisitor atom = top.visitItem_Atom();
                simple_datum(atom);
            } else if (Token.OPEN_PAREN == lookahead) {
                match(Token.OPEN_PAREN);
                top_list(top);
            } else if (
                Token.SINGLE_QUOTE == lookahead ||
                Token.BACKQUOTE == lookahead ||
                Token.COMMA == lookahead ||
                Token.SPLICE == lookahead) {

                ConsVisitor cons = top.visitItem_Cons();
                cons.visit();
                abbreviation(cons);
                cons.visitEnd();
            } else if (Token.VECTOR == lookahead) {
                VectVisitor vec = top.visitItem_Vect();
                vec.visit();
                vector(vec);
                vec.visitEnd();
            } else {
                // ERROR
                expecting("top_datum", pack(Token.BOOL, Token.NUM, Token.CHAR, Token.STRING, Token.ID, Token.OPEN_PAREN, Token.SINGLE_QUOTE, Token.BACKQUOTE, Token.COMMA, Token.SPLICE));
            }
        }

        void top_list(VectVisitor top)
        {
            if (Token.CLOSE_PAREN == lookahead) {
                match(Token.CLOSE_PAREN);
                top.visitItem();
            } else if (
                Token.BOOL == lookahead ||
                Token.NUM == lookahead ||
                Token.CHAR == lookahead ||
                Token.STRING == lookahead ||
                Token.ID == lookahead ||
                Token.OPEN_PAREN == lookahead ||
                Token.SINGLE_QUOTE == lookahead ||
                Token.BACKQUOTE == lookahead ||
                Token.COMMA == lookahead ||
                Token.SPLICE == lookahead ||
                Token.VECTOR == lookahead) {

                ConsVisitor cons = top.visitItem_Cons();
                cons.visit();
                list_contents(cons);
                cons.visitEnd();
                match(Token.CLOSE_PAREN);
            } else {
                // ERROR
                expecting("top_nil_or_list", pack(Token.CLOSE_PAREN, Token.BOOL, Token.NUM, Token.CHAR, Token.STRING, Token.ID, Token.OPEN_PAREN, Token.SINGLE_QUOTE, Token.BACKQUOTE, Token.COMMA, Token.SPLICE, Token.VECTOR));
            }
        }

        void datum(ConsVisitor cons, bool set_car)
        {
            if (Token.BOOL == lookahead ||
                Token.NUM == lookahead ||
                Token.CHAR == lookahead ||
                Token.STRING == lookahead ||
                Token.ID == lookahead) {

                AtomVisitor atom = set_car ? cons.visit_Atom_car() : cons.visit_Atom_cdr();
                //atom.visit();
                simple_datum(atom);
                //atom.visitEnd();
            } else if (Token.OPEN_PAREN == lookahead) {
                match(Token.OPEN_PAREN);
                list(cons, set_car);
            } else if (
                Token.SINGLE_QUOTE == lookahead ||
                Token.BACKQUOTE == lookahead ||
                Token.COMMA == lookahead ||
                Token.SPLICE == lookahead) {

                ConsVisitor new_cons = set_car ? cons.visit_Cons_car() : cons.visit_Cons_cdr();
                new_cons.visit();
                abbreviation(new_cons);
                new_cons.visitEnd();
            } else if (Token.VECTOR == lookahead) {
                VectVisitor vec = set_car ? cons.visit_Vect_car() : cons.visit_Vect_cdr();
                vec.visit();
                vector(vec);
                vec.visitEnd();
            } else {
                // ERROR
                expecting("datum", pack(Token.BOOL, Token.NUM, Token.CHAR, Token.STRING, Token.ID, Token.OPEN_PAREN, Token.SINGLE_QUOTE, Token.BACKQUOTE, Token.COMMA, Token.SPLICE));
            }
        }

        void simple_datum(AtomVisitor atom)
        {
            if (Token.BOOL == lookahead) {
                atom.visit((Boolean)m_attrib.value);
                match(Token.BOOL);
            } else if (Token.NUM == lookahead) {
                if (m_attrib.value is Int64) {
                    atom.visit((Int64)m_attrib.value);
                } else if (m_attrib.value is Double) {
                    atom.visit((Double)m_attrib.value);
                } else {
                    throw new InvalidOperationException();
                }

                match(Token.NUM);
            } else if (Token.CHAR == lookahead) {
                atom.visit((Char)m_attrib.value);
                match(Token.CHAR);
            } else if (Token.STRING == lookahead) {
                atom.visit((String)m_attrib.value);
                match(Token.STRING);
            } else if (Token.ID == lookahead) {
                atom.visit((Symbol)m_attrib.value);
                match(Token.ID);
            } else {
                // ERROR
                expecting("simple_datum", pack(Token.BOOL, Token.NUM, Token.CHAR, Token.STRING, Token.ID));
            }
        }

        void list(ConsVisitor cons, bool set_car)
        {
            if (Token.CLOSE_PAREN == lookahead) {
                match(Token.CLOSE_PAREN);

                if (set_car) {
                    cons.visit_car();
                } else {
                    cons.visit_cdr();
                }
            } else if (
                Token.BOOL == lookahead ||
                Token.NUM == lookahead ||
                Token.CHAR == lookahead ||
                Token.STRING == lookahead ||
                Token.ID == lookahead ||
                Token.OPEN_PAREN == lookahead ||
                Token.SINGLE_QUOTE == lookahead ||
                Token.BACKQUOTE == lookahead ||
                Token.COMMA == lookahead ||
                Token.SPLICE == lookahead ||
                Token.VECTOR == lookahead) {

                ConsVisitor new_cons = set_car ? cons.visit_Cons_car() : cons.visit_Cons_cdr();
                new_cons.visit();
                list_contents(new_cons);
                new_cons.visitEnd();
                match(Token.CLOSE_PAREN);
            } else {
                // ERROR
                expecting("list", pack(Token.CLOSE_PAREN, Token.BOOL, Token.NUM, Token.CHAR, Token.STRING, Token.ID, Token.OPEN_PAREN, Token.SINGLE_QUOTE, Token.BACKQUOTE, Token.COMMA, Token.SPLICE, Token.VECTOR));
            }
        }

        void list_contents(ConsVisitor cons)
        {
            if (Token.BOOL == lookahead ||
                Token.NUM == lookahead ||
                Token.CHAR == lookahead ||
                Token.STRING == lookahead ||
                Token.ID == lookahead ||
                Token.OPEN_PAREN == lookahead ||
                Token.SINGLE_QUOTE == lookahead ||
                Token.BACKQUOTE == lookahead ||
                Token.COMMA == lookahead ||
                Token.SPLICE == lookahead ||
                Token.VECTOR == lookahead) {

                datum(cons, true);
                list_contents_tail(cons);
            } else {
                // ERROR
                expecting("list_contents", pack(Token.BOOL, Token.NUM, Token.CHAR, Token.STRING, Token.ID, Token.OPEN_PAREN, Token.SINGLE_QUOTE, Token.BACKQUOTE, Token.COMMA, Token.SPLICE));
            }
        }

        void list_contents_tail(ConsVisitor cons)
        {
            if (Token.CLOSE_PAREN == lookahead) {
                // EPSILON
                cons.visit_cdr();
            } else if (
                Token.BOOL == lookahead ||
                Token.NUM == lookahead ||
                Token.CHAR == lookahead ||
                Token.STRING == lookahead ||
                Token.ID == lookahead ||
                Token.OPEN_PAREN == lookahead ||
                Token.SINGLE_QUOTE == lookahead ||
                Token.BACKQUOTE == lookahead ||
                Token.COMMA == lookahead ||
                Token.SPLICE == lookahead ||
                Token.VECTOR == lookahead) {

                ConsVisitor cdr = cons.visit_Cons_cdr();
                cdr.visit();
                datum(cdr, true);
                list_contents_tail(cdr);
                cdr.visitEnd();
            } else if (Token.DOT == lookahead) {
                dot_tail(cons);
            } else {
                // ERROR
                expecting("list_contents", pack(Token.DOT, Token.CLOSE_PAREN, Token.BOOL, Token.NUM, Token.CHAR, Token.STRING, Token.ID, Token.OPEN_PAREN, Token.SINGLE_QUOTE, Token.BACKQUOTE, Token.COMMA, Token.SPLICE));
            }
        }

        void dot_tail(ConsVisitor cons)
        {
            if (Token.DOT == lookahead) {
                match(Token.DOT);
                datum(cons, false);
            } else {
                // ERROR
                expecting("dot_tail", pack(Token.DOT));
            }
        }

        void abbreviation(ConsVisitor cons)
        {
            if (Token.SINGLE_QUOTE == lookahead ||                
                Token.BACKQUOTE == lookahead ||
                Token.COMMA == lookahead ||
                Token.SPLICE == lookahead) {

                abbrev_prefix(cons);

                ConsVisitor cdr = cons.visit_Cons_cdr();
                cdr.visit();
                datum(cdr, true);
                cdr.visit_cdr();
                cdr.visitEnd();
            } else {
                // ERROR
                expecting("abbreviation", pack(Token.SINGLE_QUOTE, Token.BACKQUOTE, Token.COMMA, Token.SPLICE));
            }
        }

        void abbrev_prefix(ConsVisitor abbrev)
        {
            if (Token.SINGLE_QUOTE == lookahead) {
                expand_abbrev(abbrev, "quote");
                match(Token.SINGLE_QUOTE);
            } else if (Token.BACKQUOTE == lookahead) {
                expand_abbrev(abbrev, "quasiquotation");
                match(Token.BACKQUOTE);
            } else if (Token.COMMA == lookahead) {
                expand_abbrev(abbrev, "unquote");
                match(Token.COMMA);
            } else if (Token.SPLICE == lookahead) {
                expand_abbrev(abbrev, "unquote-splicing");
                match(Token.SPLICE);
            } else {
                // ERROR
                expecting("abbrev_prefix", pack(Token.CLOSE_PAREN, Token.BOOL, Token.NUM, Token.CHAR, Token.STRING, Token.ID, Token.OPEN_PAREN, Token.SINGLE_QUOTE, Token.BACKQUOTE, Token.COMMA, Token.SPLICE));
            }
        }

        void expand_abbrev(ConsVisitor abbrev, string name)
        {
            AtomVisitor atom = abbrev.visit_Atom_car();
            //atom.visit();

            Symbol sym = Symbol.get_symbol(name);
            atom.visit(sym);

            //atom.visitEnd();
        }

        void vector(VectVisitor vec)
        {
            if (Token.VECTOR == lookahead) {
                match(Token.VECTOR);
                vector_contents(vec);
                match(Token.CLOSE_PAREN);
            } else {
                // ERROR;
                expecting("vector", pack(Token.VECTOR));
            }
        }

        void vector_contents(VectVisitor vec)
        {
            if (Token.CLOSE_PAREN == lookahead) {
                // EPSILON
            } else if (
                Token.BOOL == lookahead ||
                Token.NUM == lookahead ||
                Token.CHAR == lookahead ||
                Token.STRING == lookahead ||
                Token.ID == lookahead ||
                Token.OPEN_PAREN == lookahead ||
                Token.SINGLE_QUOTE == lookahead ||
                Token.BACKQUOTE == lookahead ||
                Token.COMMA == lookahead ||
                Token.SPLICE == lookahead ||
                Token.VECTOR == lookahead) {

                datum_list(vec);

                if (Token.CLOSE_PAREN != lookahead) {
                    // ERROR
                    expecting("vector_contents", pack(Token.CLOSE_PAREN));
                }

            } else {
                // ERROR
                expecting("vector_contents", pack(Token.CLOSE_PAREN, Token.BOOL, Token.NUM, Token.CHAR, Token.STRING, Token.ID, Token.OPEN_PAREN, Token.SINGLE_QUOTE, Token.BACKQUOTE, Token.COMMA, Token.SPLICE, Token.VECTOR));
            }
        }
    }
}
