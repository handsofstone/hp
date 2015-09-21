using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects.DataClasses;

namespace HP
{
    public partial class HPEntities
    {
        //[EdmFunction("HPModel.Store", "IsValidUser")]
        //public bool? IsValidUser(string userName, string password)
        //{
        //    //var objectContext = ((IObjectContextAdapter)this).ObjectContext;

        //    //return objectContext.CreateQuery<Boolean>(
        //    //    "dbo.IsValidUser",
        //    //    new ObjectParameter[2] {
        //    //        new ObjectParameter("username",userName),
        //    //        new ObjectParameter("password",password)
        //    //    }).Execute(MergeOption.NoTracking).FirstOrDefault();
        //    throw new NotSupportedException("Direct calls are not supported.");
        //}
    }
}