using System;
using System.Collections.Generic;


public sealed class Tuple<T1, T2, T3 , T4>
{
    private readonly T1 item1;
    private readonly T2 item2;
    private readonly T3 item3;
    private readonly T4 item4;

    public T1 Item1
    {
        get { return item1; }
    }

    public T2 Item2
    {
        get { return item2; }
    }
    public T3 Item3
    {
        get { return item3; }
    }

    public T4 Item4
    {
        get { return item4; }
    }
    public Tuple(T1 item1, T2 item2, T3 item3 , T4 item4)
    {
        this.item1 = item1;
        this.item2 = item2;
        this.item3 = item3;
        this.item4 = item4;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        hash = hash * 23 + (item1 == null ? 0 : item1.GetHashCode());
        hash = hash * 23 + (item2 == null ? 0 : item2.GetHashCode());
        hash = hash * 23 + (item3 == null ? 0 : item3.GetHashCode());
        hash = hash * 23 + (item3 == null ? 0 : item4.GetHashCode());
        return hash;
    }

    public override bool Equals(object o)
    {
        if (!(o is Tuple<T1, T2, T3,T4>))
        {
            return false;
        }

        var other = (Tuple<T1, T2, T3,T4>)o;

        return this == other;
    }

    public static bool operator ==(Tuple<T1, T2, T3,T4> a, Tuple<T1, T2, T3,T4> b)
    {
        if (object.ReferenceEquals(a, null))
        {
            return object.ReferenceEquals(b, null);
        }
        if (a.item1 == null && b.item1 != null) return false;
        if (a.item2 == null && b.item2 != null) return false;
        if (a.item3 == null && b.item3 != null) return false;
        if (a.item4 == null && b.item4 != null) return false;
        return
            a.item1.Equals(b.item1) &&
            a.item2.Equals(b.item2) &&
            a.item3.Equals(b.item3) &&
             a.item4.Equals(b.item4);
    }

    public static bool operator !=(Tuple<T1, T2, T3,T4> a, Tuple<T1, T2, T3,T4> b)
    {
        return !(a == b);
    }

    public void Unpack(Action<T1, T2, T3> unpackerDelegate)
    {
        unpackerDelegate(Item1, Item2, Item3);
    }
}
