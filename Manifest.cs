using Intersect.Plugins.Interfaces;
using Intersect.Plugins.Manifests.Types;
using Intersect.Utilities;
using Semver;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace InGameClock
{
    [StructLayout(LayoutKind.Sequential, Size = 1)]
    public struct Manifest : IManifestHelper, IEquatable<IManifestHelper>, IEquatable<Manifest>
    {
        public string Name => typeof(Manifest).Namespace;

        public string Key => typeof(Manifest).Namespace;

        public SemVersion Version => new SemVersion(0, 3, 3);

        public Authors Authors => "Arufonsu";

        public string Homepage => "https://github.com/Arufonsu";

        public override bool Equals(object obj) => obj is Manifest other && Equals(other) ||
                                                   obj is IManifestHelper otherManifestHelper &&
                                                   Equals(otherManifestHelper);

        public override int GetHashCode() => ValueUtils.ComputeHashCode(Name, Key, Version, Authors, Homepage);

        public static bool operator ==(Manifest left, Manifest right) => left.Equals(right);

        public static bool operator !=(Manifest left, Manifest right) => !(left == right);

        public bool Equals(Manifest other) => Equals(other as IManifestHelper);

        public bool Equals(IManifestHelper other) => other != null &&
                                                     string.Equals(Name, other.Name, StringComparison.Ordinal) &&
                                                     string.Equals(Key, other.Key, StringComparison.Ordinal) &&
                                                     Version.Equals(other.Version) &&
                                                     Authors.Equals(other.Authors as IEnumerable<Author>) &&
                                                     string.Equals(Homepage, other.Homepage,
                                                         StringComparison.OrdinalIgnoreCase);
    }
}
