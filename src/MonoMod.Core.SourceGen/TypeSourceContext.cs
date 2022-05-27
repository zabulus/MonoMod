﻿using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoMod.Core.SourceGen {
    internal class TypeSourceContext {
        private readonly List<INamedTypeSymbol> containingTypes = new();
        private readonly string? namespaceName;

        public TypeSourceContext(ISymbol working) {
            INamedTypeSymbol? outerType = null;
            ISymbol currentSym = working;
            while (currentSym.ContainingType is not null) {
                outerType = currentSym.ContainingType;
                containingTypes.Add(outerType);
                currentSym = outerType;
            }

            var ns = outerType?.ContainingNamespace;
            if (ns is null) {
                namespaceName = null;
            } else {
                namespaceName = ns.ToDisplayString();
            }

            containingTypes.Reverse();
        }

        public string FullContextName => namespaceName + (namespaceName is not null ? "." : "") + string.Join(".", containingTypes.Select(t => t.Name));

        public void AppendEnterContext(CodeBuilder builder) {
            if (namespaceName != null) {
                builder.WriteLine($"namespace {namespaceName} {{")
                    .IncreaseIndent();
            }

            foreach (var type in containingTypes) {
                var isRec = type.IsRecord;
                var isStruct = type.IsValueType;
                var isRef = type.IsReferenceType;

                builder.WriteLine($"partial {(isRec ? "record" : "")}{(isRef && !isRec ? "class" : "")} {(isStruct ? "struct" : "")} {type.Name} {{")
                    .IncreaseIndent();
            }
        }

        public void AppendExitContext(CodeBuilder builder) {
            for (var i = 0; i < containingTypes.Count; i++) {
                builder.DecreaseIndent().WriteLine($"}}");
            }

            if (namespaceName != null) {
                builder.DecreaseIndent().WriteLine($"}}");
            }
        }
    }
}
