namespace UniversityApp.Model.Entities;

public abstract class Entity
{
    abstract public Guid Id { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity other || GetType() != obj.GetType())
        {
            return false;
        }

        return this.Id == other.Id;
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Equals(right);
    }
    public static bool operator !=(Entity? left, Entity? right)
    {
        return !(left == right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
