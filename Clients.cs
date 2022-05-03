using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examen
{
    internal class Clients
    {
        /// <summary>
        /// 
        /// Этот класс нужен для того чтобы постоянно заного не создавать подключение к бд
        /// То есть один раз создали и потом дергаем это подключение, так и проще и быстрее
        /// Обращаться потом к нему нужно так - 
        /// 
        /// Clients.GetClient().'здесь пишешь какую сущность хочешь вытянуть из базы данных пользователей или 
        /// услуги какие то, кавыычки не нужны'.'а здесь уже пишешь в каком виде тебе их выдать, т.е. отсортировать или кого то конкретного'.ToList()
        /// 
        /// </summary>
        public static Entities entities;
        public static Entities GetClients()
        {
            if (entities == null)
            {
                entities = new Entities();
            }
            return entities;

        }

    }
}
