using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.SignalR;
using Ninject.Activation;
using x_nova_template.Areas.Admin;
using x_nova_template.Models;
using Microsoft.AspNet.SignalR.Hubs;

namespace x_nova_template.Hubs
{
    [HubName("xnovaHub")]
    public class XnovaHub : Hub
    {

        public void Send(string message, string connId)
        {


            if (message.Contains("<script>"))
            {
                throw new HubException("Некорректное сообщение", new { user = Context.User.Identity.Name, message = message });
            }
            if (string.IsNullOrWhiteSpace(message))
            {
                //Clients.Caller.ShowErrorMessage("Пустое сообщение!");
                throw new HubException("Пустое сообщение!", new { user = Context.User.Identity.Name, message = message });
            }
            else
            {
                var mess = HttpUtility.HtmlEncode(message);
                var connid = HttpUtility.HtmlEncode(connId);

                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    LiveUser user = null;
                    LiveUser user1 = null;
                    LiveUser dialogUser = null;
                    user1 = db.LiveUsers.SingleOrDefault(x => x.IsAdmin);
                    user = db.LiveUsers.SingleOrDefault(x => x.ConnId == connid);
                    dialogUser = db.LiveUsers.SingleOrDefault(x => x.ConnId == user1.GroupId);
                    // Тест работы виджета
                    //

                    Clients.Client(user1.ConnId).Test(user1.GroupId, user1.ConnId, connid);
                    if (!Context.User.IsInRole("admin"))
                    {
                        //Сообщение пользователя
                        //

                        if (user.ConnId == user1.GroupId)
                        {
                            //Прямой диалог
                            //
                            var newM = new Message()
                            {
                                TextMess = mess,
                                DateAdded = DateTime.Now,
                                Visited = true,
                                UserID = user.UserID
                            };
                            user.LiveMessages.Add(newM);

                        }
                        else
                        {
                            //Сообщение в очередь
                            //
                            var newM = new Message()
                            {
                                TextMess = mess,
                                DateAdded = DateTime.Now,
                                Visited = false,
                                UserID = user.UserID
                            };
                            user.LiveMessages.Add(newM);
                            db.SaveChanges();
                            var newQuestions = db.LiveUsers.Where(x => x.LiveMessages.Any(y => !y.Visited)).Count();
                            Clients.Client(user1.ConnId)
                               .NotifyNewQuestion(user.UserName, user.LiveMessages.Where(x => !x.Visited).Count(),
                               user.ConnId, newQuestions);
                        }

                        db.SaveChanges();
                        //Публикуем сообщение в группу пользователя
                        //
                        Clients.Group(user.ConnId).AddNewMessageToPage(DateTime.Now.ToString("dd MMM, hh:mm"),
                           user.UserName, mess, false);



                    }
                    else
                    {

                        if (!db.LiveUsers.Any(x => x.ConnId == user1.GroupId))
                        {

                            throw new HubException("Вас никто не видит", new { user = Context.User.Identity.Name, message = message });
                        }
                        else
                        {
                            //Публикуем сообщение консультанта
                            //

                            Clients.Group(user1.GroupId).AddNewMessageToPage(DateTime.Now.ToString("dd MMM, hh:mm"),
                               "Консультант", mess, true);
                            Clients.Client(dialogUser.ConnId).NotifyAdmAnswear();
                        }
                    }
                }
            }
        }

            public async void SendUserRoom(string conn)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var user = db.LiveUsers.SingleOrDefault(x => x.ConnId == conn);
                    var user1 = db.LiveUsers.SingleOrDefault(x => (bool)x.IsAdmin);
                    user1.GroupId = conn;
                    user.LiveMessages.ToList().ForEach(x => x.Visited = true);
                    db.SaveChanges();
                    var newQuestions = db.LiveUsers.Where(x => x.LiveMessages.Any(y => !y.Visited)).Count();
                    // Отсылка вопроса прользователя
                    await Clients.Client(user1.ConnId).GetUserRoom(user.UserName, user.LiveMessages.ToList(), user.ConnId);
                }
            }

            public override Task OnConnected()
            {

                LiveUser user = null;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {

                    user = db.LiveUsers.SingleOrDefault(x => x.IsAdmin);
                    if (user == null)
                    {
                        db.LiveUsers.Add(new LiveUser
                        {

                            UserName = "admin",
                            Email = "adm@adm.ru",
                            IsAdmin = true,
                            IsOnline = false,
                            FeedMessage = "txt"

                        });
                    }

                    db.SaveChanges();

                }

                var connId = Context.ConnectionId;


                 return base.OnConnected();
            }
            /* public void CollapseChatroom() {
                 using (ApplicationDbContext db = new ApplicationDbContext())
                 {
                     var user = db.LiveUsers.SingleOrDefault(x => x.IsAdmin);
                     var user1 = db.LiveUsers.SingleOrDefault(x => x.ConnId == Context.ConnectionId);
                     if (Context.User.IsInRole("Admin"))
                     {
                         foreach (var item in db.LiveUsers.Where(x => !x.IsAdmin))
                         {
                             db.LiveUsers.Remove(item);
                         }
                         user.IsOnline = false;
                         db.SaveChanges();
                    
                         Clients.All.AdminOut("Консультант отключился.");
                         Clients.Caller.Out();
                     }
                     else
                     {

                         Clients.Group(user1.ConnId).UserOut("Пользователь отключился.");
                         Clients.Caller.Out();
                         Clients.All.NotifyAboutDisc(user1.ConnId);
                         Groups.Remove(user1.ConnId, user1.ConnId);

                         db.LiveUsers.Remove(user1);
                         db.SaveChanges();
                     }
                 }
             }*/

            public  override Task OnDisconnected(bool stopCalled)
            {
                if (!stopCalled) { }
                else
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {

                        var user = db.LiveUsers.SingleOrDefault(x => x.IsAdmin);
                        var user1 = db.LiveUsers.SingleOrDefault(x => x.ConnId == Context.ConnectionId);
                        if (Context.User.IsInRole("admin"))
                        {
                            foreach (var item in db.LiveUsers.Where(x => !x.IsAdmin))
                            {
                                db.LiveUsers.Remove(item);
                            }
                            user.IsOnline = false;
                            user.GroupId = "";
                            db.SaveChanges();
                            Groups.Remove(user.ConnId, user1.GroupId);
                            Clients.All.AdminOut("Консультант отключился.");
                        }
                        else
                        {


                            var newQuestions = db.LiveUsers.Where(x => x.LiveMessages.Any(y => !y.Visited)).Count() - 1;
                            var isInGroup = user.GroupId == Context.ConnectionId;
                             Clients.Client(user.ConnId).UserOut(Context.ConnectionId,
                                "Пользователь " + user1.UserName + " отключился ", newQuestions, isInGroup);
                             Groups.Remove(user1.ConnId, user1.ConnId);

                            db.LiveUsers.Remove(user1);
                            db.SaveChanges();
                             Clients.Client(ConsultantController.ConsultConnId()).NotifyAboutConnection(
                                 ConsultantController.OnlineUsersCount());
                        }
                    }
                }
                return  base.OnDisconnected(stopCalled);
            }
            private LiveUser GetAdmin() {
                ApplicationDbContext db = new ApplicationDbContext();
                var user = db.LiveUsers.SingleOrDefault(x => x.IsAdmin);
                return user;
            }
            
            public void AdminOff(){
                Clients.Caller.ConsultantOff();
            }
            public  void NotifyAboutConnect()
            {
                 Clients.Client(ConsultantController.ConsultConnId()).NotifyAboutConnection(ConsultantController.OnlineUsersCount());
            }
            public  void JoinGroup(string connId, int isAdm)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var user = db.LiveUsers.SingleOrDefault(x => x.IsAdmin);

                    if (isAdm > 0)
                    {

                         Groups.Add(user.ConnId, user.GroupId);
                    }
                    else
                    {
                         Groups.Add(connId, connId);
                        //Clients.All.AfterGroup(connId);
                    }
                }
            }

            public  void LeaveGroup(string connId)
            {
                 Groups.Remove(connId, connId);
            }
        }
    }
