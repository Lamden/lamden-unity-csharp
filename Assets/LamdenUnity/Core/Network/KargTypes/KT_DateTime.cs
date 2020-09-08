using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LamdenUnity
{
    [Serializable]
    public class KT_DateTime : KwargType
    {        
        public int[] date;

        public KT_DateTime(string dateTimeString)
        {
            DateTime dateTime;
            try
            {
                dateTime = DateTime.Parse(dateTimeString);
                date = EncodeDateTime(dateTime);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error decoding date string '{dateTimeString}': {ex.Message}");               
            }            
        }

        public KT_DateTime(DateTime dateTime)
        {        
             date = EncodeDateTime(dateTime);         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime">Local date time</param>
        /// <returns></returns>
        private int[] EncodeDateTime(DateTime dateTime)
        {
            DateTime utc = dateTime.ToUniversalTime();
            return new int[]{
                utc.Year,
                utc.Month,
                utc.Day,
                utc.Hour,
                utc.Minute,
                utc.Second,
                utc.Millisecond
            };
        }

        public override string ToString()
        {
            return $"[{date[0]},{date[1]},{date[2]},{date[3]},{date[4]},{date[5]},{date[6]}]";
        }

    }
}
