﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ERP.BusinessEntity
{
    [DataContract]
    public class CommimentDetailBE
    {
        #region "Atributos"
        [DataMember]
        public Int32 IdCommimentDetail { get; set; }
        [DataMember]
        public Int32 IdCommiment { get; set; }
        [DataMember]
        public Int32 IdStyle { get; set; }
        [DataMember]
        public Decimal Quantity { get; set; }
        [DataMember]
        public Decimal Fob { get; set; }
        [DataMember]
        public Decimal Total { get; set; }
        [DataMember]
        public Boolean FlagState { get; set; }
        [DataMember]
        public String Login { get; set; }
        [DataMember]
        public String Machine { get; set; }

        //DATOS EXTERNOS
        [DataMember]
        public Int32 IdCompany { get; set; }
        [DataMember]
        public String NameStyle { get; set; }
        [DataMember]
        public String Description { get; set; }
        [DataMember]
        public Int32 TipoOper { get; set; }


        #endregion
    }
}
