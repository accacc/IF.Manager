using System;
using System.Collections.Generic;
using System.Text;

namespace IF.Manager.Contracts.Enum
{
    public enum QueryFilterOperator
    {
        Equal=0,
        NotEqual=1,
        StringContains=2,
        Null=3,
        NotNull=4,
        Greater=5,
        Less=6,
        StartWith=7,
        EndWith=8,
        GreaterAndEqual=9,
        LessAndEqual=10


    }
}
