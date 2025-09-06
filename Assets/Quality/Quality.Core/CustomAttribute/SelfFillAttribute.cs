using System;
using UnityEngine;

namespace Quality.Core.CustomAttribute
{
    public enum SearchTarget
    {
        SELF,
        PARENT,
        CHILDREN,
        SCENE,
        RESOURCE
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SelfFillAttribute : PropertyAttribute
    {
        public readonly SearchTarget Target;
        public readonly bool         IncludeInactive;
        public readonly string       Path;

        public SelfFillAttribute(SearchTarget target = SearchTarget.SELF, bool includeInactive = true)
        {
            this.Target          = target;
            this.IncludeInactive = includeInactive;
        }

        public SelfFillAttribute(string path)
        {
            this.Target = SearchTarget.RESOURCE;
            this.Path   = path;
        }
    }
}
