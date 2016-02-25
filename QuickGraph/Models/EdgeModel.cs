using QuickGraph;

public class EdgeModel : IEdge<VertextModel>
{
    public EdgeModel(VertextModel source, VertextModel target)
    {
        Source = source;
        Target = target;
    }


    public string SourceRole { get; set; }
    public string TargetRole { get; set; }
    public VertextModel Source { get; }
    public VertextModel Target { get; }

    protected bool Equals(EdgeModel other)
    {
        return string.Equals(SourceRole, other.SourceRole) && string.Equals(TargetRole, other.TargetRole) && Equals(Source, other.Source) && Equals(Target, other.Target);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((EdgeModel) obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (SourceRole != null ? SourceRole.GetHashCode() : 0);
            hashCode = (hashCode*397) ^ (TargetRole != null ? TargetRole.GetHashCode() : 0);
            hashCode = (hashCode*397) ^ (Source != null ? Source.GetHashCode() : 0);
            hashCode = (hashCode*397) ^ (Target != null ? Target.GetHashCode() : 0);
            return hashCode;
        }
    }
}