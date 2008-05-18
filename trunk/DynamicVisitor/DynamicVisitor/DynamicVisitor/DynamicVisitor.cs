using GuidedTour;

namespace DynamicVisitor {
    static class DynamicVisitor {
        public static void accept(object graph, object visitor)
        {
            Guide guide = new Guide();
            VisitSiteseer vss = new VisitSiteseer(visitor);
            vss.AddAlias("System.Object[]", "Vector");
            guide.tour(graph, vss);
        }
    }
}
