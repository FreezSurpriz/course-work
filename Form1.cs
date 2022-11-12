using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Text.Json.Serialization;
//using Windows.Web.Http;
using System.IO.Compression;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cursash2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<double[]> ls = new List<double[]>();
        double[] res1; double[] res2;
        static async Task<string> func(string spec, string year, int un, int qua, int bas)
        {
            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            // In production code, don't destroy the HttpClient through using, but better reuse an existing instance
            // https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            using (var httpClient = new HttpClient(handler))
            {
                string site;
                if (year != "2021")
                    site = year + ".edbo.gov.ua/offers/?qualification=1&education-base=40&speciality=121&education-form=1&course=1&university-name=" + un;
                else
                    site = year + ".edbo.gov.ua/offers-universities/";
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://vstup" + site))
                {
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:97.0) Gecko/20100101 Firefox/97.0");
                    request.Headers.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                    request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, br");
                    request.Headers.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                    request.Headers.TryAddWithoutValidation("Origin", "https://vstup" + year + ".edbo.gov.ua");
                    request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
                    request.Headers.TryAddWithoutValidation("Referer", "https://vstup" + year + ".edbo.gov.ua/offers/?qualification=1&education-base=40&speciality=121&education-form=1&course=1&university-name=" + un);
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
                    request.Headers.TryAddWithoutValidation("Cookie", "_ga=GA1.1.2041819962.1638457784; _ga_W6WT1K3VXZ=GS1.1.1644952579.3.1.1644952591.0; _ga_YC32TV7WL7=GS1.1.1644952590.4.1.1644952753.0; _ga_96Q7K30V0N=GS1.1.1643115886.4.0.1643115888.0; PHPSESSID=sof0jmp661095i2grv7rhbha8a");
                    if (year != "2021")
                        request.Content = new StringContent("action=universities&university-id=&qualification=" + qua + "&education-base=" + bas + "&speciality=" + spec + "&program=&education-form=1&course=1&region=&university-name=" + un);
                    else
                        request.Content = new StringContent("qualification=" + qua + "&education_base=" + bas + "&speciality=" + spec + "&region=80&university=" + un + "&study_program=&education_form=&course=");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded; charset=UTF-8");


                    var response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    response.Content.ReadAsStringAsync().Wait();
                    var responseBody = response.Content.ReadAsStreamAsync().Result;
                    Stream decompressed = new GZipStream(responseBody, CompressionMode.Decompress);
                    StreamReader objReader = new StreamReader(decompressed, Encoding.UTF8);
                    //var response = JsonConvert.DeserializeObject<T>(responseBody);
                    var resp = objReader.ReadToEnd();
                    return resp;
                }
            }
        }
        static async Task<string> func2(string codes, string year, int un)
        {
            var handler = new HttpClientHandler();
            handler.UseCookies = false;

            // In production code, don't destroy the HttpClient through using, but better reuse an existing instance
            // https://www.aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
            using (var httpClient = new HttpClient(handler))
            {
                string site;
                if (year != "2021")
                    site = year + ".edbo.gov.ua/";
                else
                    site = year + ".edbo.gov.ua/offers-list/";
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://vstup" + site))
                {
                    request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:97.0) Gecko/20100101 Firefox/97.0");
                    request.Headers.TryAddWithoutValidation("Accept", "application/json, text/javascript, */*; q=0.01");
                    request.Headers.TryAddWithoutValidation("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.5,en;q=0.3");
                    request.Headers.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate, br");
                    request.Headers.TryAddWithoutValidation("X-Requested-With", "XMLHttpRequest");
                    request.Headers.TryAddWithoutValidation("Origin", "https://vstup" + year + ".edbo.gov.ua");
                    request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
                    request.Headers.TryAddWithoutValidation("Referer", "https://vstup" + year + ".edbo.gov.ua/offers/?qualification=1&education-base=40&speciality=121&university-name=" + un);
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
                    request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "same-origin");
                    request.Headers.TryAddWithoutValidation("Cookie", "_ga=GA1.1.2041819962.1638457784; _ga_W6WT1K3VXZ=GS1.1.1644952579.3.1.1644952591.0; _ga_YC32TV7WL7=GS1.1.1644952590.4.1.1644952753.0; _ga_96Q7K30V0N=GS1.1.1643115886.4.0.1643115888.0; PHPSESSID=sof0jmp661095i2grv7rhbha8a");

                    if (year != "2021")
                        request.Content = new StringContent("action=offers&usids=" + codes);
                    else
                        request.Content = new StringContent("ids=" + codes);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded; charset=UTF-8");



                    var response = await httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                    response.Content.ReadAsStringAsync().Wait();
                    var responseBody = response.Content.ReadAsStreamAsync().Result;
                    Stream decompressed = new GZipStream(responseBody, CompressionMode.Decompress);
                    StreamReader objReader = new StreamReader(decompressed, Encoding.UTF8);
                    //var response = JsonConvert.DeserializeObject<T>(responseBody);
                    var resp = objReader.ReadToEnd();
                    return resp;
                }
            }
        }
        private async void writeout(string year, string spec, int un, int qua, int bas, int first, int second, ListBox l)
        {
            var result = await func(spec, year, un, qua, bas);
            //MessageBox.Show(result);
            if (comboBox2.SelectedItem.ToString() == "2021")
            {
                var results = result.Split('"');
                if (results.Length < second)
                {
                    //l.Items.Add("Конкурсні пропозиції не знайдені");
                    return;
                }
                result = results[second];
                //MessageBox.Show(result, second.ToString());
            }
            else
            {
                var results = result.Split('"');
                if (results.Length < first)
                {
                    //l.Items.Add("Конкурсні пропозиції не знайдені");
                    return;
                }
                result = results[first];
                //MessageBox.Show(result, second.ToString());
            }
            string[] codes = result.Split(',');
            result = await func2(result, year, un);
            //MessageBox.Show(result);
            var resp = JsonConvert.DeserializeObject<JToken>(result);
            //MessageBox.Show(resp["offers_requests_info"].ToString());
            if (year != "2021")
                foreach (string c in codes)
                {
                    //MessageBox.Show(resp["offers_requests_info"].ToString());
                    var numbers = resp["offers_requests_info"][c];
                    //MessageBox.Show(resp["offers_requests_info"].ToString());
                    //MessageBox.Show(resp["offers_requests_info"][c].ToString());
                    if ((numbers[2].ToString() != "") && (numbers[3].ToString() != "") && (numbers[4].ToString() != ""))
                    {
                        //MessageBox.Show(numbers[2].ToString());
                        ls.Add(new double[] { un, Convert.ToDouble(numbers[2].ToString()), Convert.ToDouble(numbers[3].ToString()), Convert.ToDouble(numbers[4].ToString()) });
                        l.Items.Add("Сер:" + numbers[2].ToString() + " Мін:" + numbers[3].ToString() + " Макс:" + numbers[4].ToString());
                    }

                }
            else
            {
                var respi = resp["offers"];
                //MessageBox.Show(respi.ToString());
                for (int i = 0; i < codes.Length; i++)
                {
                    //MessageBox.Show(respi[i]["st"].ToString());
                    if (respi[i]["st"] != null)
                    {

                        var numbers = respi[i]["st"]["c"];
                        if ((numbers["rm"].ToString() != "") && (numbers["obm"].ToString() != "") && (numbers["ocm"].ToString() != ""))
                        {
                            //MessageBox.Show(numbers["rm"].ToString());
                            ls.Add(new double[] { un, Convert.ToDouble(numbers["rm"].ToString()), Convert.ToDouble(numbers["obm"].ToString()), Convert.ToDouble(numbers["ocm"].ToString()) });
                            l.Items.Add("Сер:" + numbers["rm"].ToString() + " Мін:" + numbers["obm"].ToString() + " Макс:" + numbers["ocm"].ToString());

                        }
                    }

                }
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ls = new List<double[]>();
            res2 =null; res1 =null;
            string year = comboBox2.SelectedItem.ToString();
            string spec = comboBox1.SelectedItem.ToString();
            spec = spec.Split(' ')[0];
            //int qua = comboBox3.SelectedIndex;
            int qualification = 1;
            int[] bases = { 40, 320, 520 };
            //switch (qua)
            //{
            //    case (0): qualification = 1; bases = new int[] { 40, 320, 520 }; break;
            //    case (1): qualification = 2; bases = new int[] { 40, 320, 520, 640 }; break;
            //    case (2): qualification = 6; bases = new int[] { 40, 320 }; break;
            //    case (3): qualification = 9; bases = new int[] { 30, 40, 510, 520 }; break;
            //}
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            string code1 = "73" + year + spec + qualification;
            double[] res = search(code1);
            if (res.Length != 1)
            {
                res1 = res;
            }
            else
            foreach (int bas in bases)
            {
                //znu

                writeout(year, spec, 73, qualification, bas, 5, 11, listBox1);
                
            }
            string code2 = "91" + year + spec + qualification;
            res = search(code2);
            if (res.Length != 1)
            {
                res2 = res;
            }
            else
            foreach (int bas in bases)
            {
                //zntu

                writeout(year, spec, 91, qualification, bas, 7, 13, listBox2);
            }
            MessageBox.Show(ls.ToString().Length.ToString(),"Кількість пропозицій");
            double count1 = 0; double count2 = 0; double s1 = 0; double s2 = 0; double min1 = 0; double min2 = 0; double max1 = 0; double max2 = 0;
            foreach (double[] arr in ls)
            {
                if (arr[0] == 73)
                {
                    count1++;
                    s1 += arr[1];
                    min1 += arr[2];
                    max1 += arr[3];
                }
                else
                {
                    count2++;
                    s2 += arr[1];
                    min2 += arr[2];
                    max2 += arr[3];
                }
            }

            if (res1 == null && res2==null)
            {
                res1 = new double[] { Math.Round(s1 / count1, 2), Math.Round(min1 / count1, 2), Math.Round(max1 / count1, 2) };
                res2 = new double[] { Math.Round(s2 / count2, 2), Math.Round(min2 / count2, 2), Math.Round(max2 / count2, 2) };
                writein(code1, res1);
                writein(code2, res2);
            }
            else
            {
                MessageBox.Show("Сер:" + res1[0].ToString() + "Мін:" + res1[1].ToString() + "Макс:" + res1[2].ToString(), "ЗНУ");
                MessageBox.Show("Сер:" + res2[0].ToString() + "Мін:" + res2[1].ToString() + "Макс:" + res2[2].ToString(), "ЗПУ");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
        class Result 
        {
            public string code { get; set; }
            public double[] nums { get; set; }
            public Result(string c,double[] n)
            {
                code = c;nums = n;
            }
        }
        


        private void button2_Click(object sender, EventArgs e)
        {
            chart1.Series["ЗНУ"].Points[0].YValues[0] = res1[0];
            chart1.Series["ЗНУ"].Points[0].Label = res1[0].ToString();
            chart1.Series["ЗНУ"].Points[1].YValues[0] = res1[1];
            chart1.Series["ЗНУ"].Points[2].YValues[0] = res1[2];
            chart1.Series["ЗПУ"].Points[0].YValues[0] = res2[0];
            chart1.Series["ЗПУ"].Points[1].YValues[0] = res2[1];
            chart1.Series["ЗПУ"].Points[2].YValues[0] = res2[2];
            chart1.Series["ЗНУ"].Points[1].Label = res1[1].ToString();
            chart1.Series["ЗНУ"].Points[2].Label = res1[2].ToString();
            chart1.Series["ЗПУ"].Points[0].Label = res2[0].ToString();
            chart1.Series["ЗПУ"].Points[1].Label = res2[1].ToString();
            chart1.Series["ЗПУ"].Points[2].Label = res2[2].ToString();
            chart1.ResetAutoValues();
        }


        public double[] search(string code)
        {
            using (StreamReader r = new StreamReader("user.json"))
            {
                string json = r.ReadToEnd();
                List<Result> items = JsonConvert.DeserializeObject<List<Result>>(json);
                foreach (Result res in items)
                {
                    if (res.code == code)
                    {
                        return (new double[] { res.nums[0], res.nums[1], res.nums[2] });
                    }
                }
            }
            return new double[1];
        } 
        public void writein(string code,double[] reses)
        {
            List<Result> its = new List<Result>();
            using (StreamReader r = new StreamReader("user.json"))
            {
                string json = r.ReadToEnd();
                List<Result> items = JsonConvert.DeserializeObject<List<Result>>(json);
                Result res = new Result(code, reses);
                items.Add(res);
                its = items;
            }
            using (FileStream fs = new FileStream("user.json", FileMode.OpenOrCreate))
            {
                System.Text.Json.JsonSerializer.SerializeAsync<List<Result>>(fs, its);
            }
        }
    }


}
