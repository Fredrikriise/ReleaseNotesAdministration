using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReleaseNotes.Controllers
{
    public class SubscribersController : Controller
    {
        private readonly ISubscriberClient _subscriberClient;

        //public SubscribersController(ISubscriberClient subscriberClient)
        //{
        //    _subscriberClient = subscriberClient;
        //}

        public IActionResult Index()
        {
            //var subscribers = _subscriberClient.GetSubscribers();
            return View();
        }
    }
}
