using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalSpolecznosciowy.Models.HelpersClass
{
    public class UserFriends
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();


        

        /// <summary>
        /// Metoda wyswietlajaca przyjaciol uzytkownika
        /// </summary>
        /// <param name="userId">Przekazac userId uzytkownika, ktorego chcemy wyswietlic przyjaciol</param>
        /// <returns>zwraca przyjaciol uzytkownika</returns>
        public IEnumerable<ApplicationUser> ListOfFriendsUser(string userId)
        {
            List<String> friends1 = (from f in _db.Friend
                                     where f.UserId == userId && f.Accepted
                                     select f.UserFriendId
                ).ToList();

            List<String> friends2 = (from f in _db.Friend
                                     where f.UserFriendId == userId && f.Accepted
                                     select f.UserId
                ).ToList();

            IEnumerable<String> friends1JoinFriends2 = friends1.Union(friends2);

            IEnumerable<ApplicationUser> friendUserAll = from e in _db.Users where friends1JoinFriends2.Contains(e.Id) select e;

            return friendUserAll;
        }
    }
}