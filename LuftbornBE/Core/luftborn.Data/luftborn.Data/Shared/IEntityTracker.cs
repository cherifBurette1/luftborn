using System;

namespace luftborn.Data
{
    public interface IEntityTracker
    {
        DateTime CreateDate { get; set; }
        DateTime UpdateDate { get; set; }
        int Creator { get; set; }
        int Updator { get; set; }
    }
}
