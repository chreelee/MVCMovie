using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Razor;

namespace MvcMovie.Infrastructure
{
    public class FeatureViewLocationExpander : IViewLocationExpander
    {
        // takes in VIewLocationExpanderContext
        public void PopulateValues(ViewLocationExpanderContext context)
        {

        }

        // method that will expand view locations
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            // yield keyword will allow us to iterate over the selected elements
            yield return "/Features/{1}/Views/{0}.cshtml";
            yield return "/Features/Shared/{0}.cshtml";
            // when calling method, iterate thru elements and get locations one at a time
            foreach (var location in viewLocations)
            {
                yield return location;
            }
        }
    }
}
