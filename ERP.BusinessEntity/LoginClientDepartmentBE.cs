﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ERP.BusinessEntity
{
    [DataContract]
    public class LoginClientDepartmentBE
    {
        #region "Atributos"

        [DataMember]
        public Int32 IdLoginClientDepartment { get; set; }
        [DataMember]
        public Int32 IdCompany { get; set; }
        [DataMember]
        public Int32 IdLogin { get; set; }
        [DataMember]
        public Int32 IdClientDepartment { get; set; }
        [DataMember]
        public Boolean FlagState { get; set; }
        [DataMember]
        public String Login { get; set; }
        [DataMember]
        public String Machine { get; set; }
        [DataMember]
        public Int32 TipoOper { get; set; }

        //DATOS EXTERNOS
        [DataMember]
        public Int32 IdClient { get; set; }
        [DataMember]
        public String NameClient { get; set; }
        [DataMember]
        public String NameDivision { get; set; }

        #endregion

    }
}
