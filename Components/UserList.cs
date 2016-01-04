using DotNetNuke.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace DotNetNuclear.Modules.NextGenModule.Components
{
    public class PortalUserList
    {
        private int _portalId = 0;
        private List<ListOption> _userList;

        public List<ListOption> UserList { get; set; }

        public string SerializedUserList 
        {
            get
            {
                return new JavaScriptSerializer().Serialize(_userList);
            }
        }

        public PortalUserList()
        {
            populateUserList();
        }

        public PortalUserList(int portalId)
        {
            _portalId = portalId;
            populateUserList();
        }

        private void populateUserList()
        {
            _userList = new List<ListOption>();
            foreach (UserInfo u in UserController.GetUsers(_portalId))
            {
                _userList.Add(new ListOption { text = u.Username, id = u.UserID });
            }
        }
    }

    public class ListOption
    {
        public int id { get; set; }
        public string text { get; set; }
    }

}