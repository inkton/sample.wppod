using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Wppod
{
    public class Result<T>
    {
        public Result()
        {   
            ResultCode = -1;
            ResultText = "Uknown Error";
            Data = new Dictionary<string, object>();
        }

        public void SetSuccess(
            string entity,            
            T data,
            string notes = null)
        {
            ResultCode = 0;
            ResultText = "NEST_RESULT_SUCCESS";
            Data[entity] = data;
            Notes = notes;
        }

        public void SetSuccess(
            string entity,
            List<T> data,
            string notes = null)
        {
            ResultCode = 0;
            ResultText = "NEST_RESULT_SUCCESS";
            Data[entity] = data;
            Notes = notes;
        }

        public void SetFail(
            string notes = null)
        {
            ResultCode = -200;
            ResultText = "NEST_RESULT_ERROR";
            Notes = notes;
        }

        [JsonProperty("notes")]
        public string Notes { get; set; }
        [JsonProperty("result_code")]
        public int ResultCode { get; set; }
        [JsonProperty("result_text")]
        public string ResultText { get; set; }
        [JsonProperty("data")]
        public Dictionary<string, object> Data { get; set; }
    }
}