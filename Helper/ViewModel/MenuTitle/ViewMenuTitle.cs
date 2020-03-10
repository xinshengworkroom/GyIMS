using GyIMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace GyIMS.Helper.ViewModel.MenuTitles
{
    public class ViewMenuTitle
    {
        public MenuTitle _MenuTitle = new MenuTitle();

        public MapsMenuTitle _MapsMenuTitle = new MapsMenuTitle();

        public StatusInfo _StatusInfo = new StatusInfo();

    }

    [DataContract]
    public class linqViewMenuTitle
    {
        [DataMember]
        public string ID { get; set; }
        [DataMember]
        public string TitleName { get; set; }
        [DataMember]
        public string Rank { get; set; }
        [DataMember]
        public string SN { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string Url { get; set; }
    }
}