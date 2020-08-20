using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Enum
{

    public enum EntityRelationDirectionType
    {
        One=0,
        Many=1
    }
    public enum EntityRelationType
    {
        None = -1,
        OneToMany = 0,
        OneToOne= 1,
        ManyToMany=2
    }
}
