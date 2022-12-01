using BezpieczeństwoSystemów.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BezpieczeństwoSystemów.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static long? p { get; set; }
        public static long? q { get; set; }
        public static long? o { get; set; }
        public static long? e { get; set; }
        public static long? n { get; set; }
        public static long? d { get; set; }
        public static long? c { get; set; }
        public static long? t { get; set; }

        public static string? message { get; set; }
        public static string? messageHashed { get; set; }
        public static long[] messageHashedArr { get; set; }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private bool PrimeTest(long x)
        {
            int i = 1;
            while (Math.Pow(i, 2) < x)
                if (x % ++i == 0)
                    return true;
            return false;
        }
        private long GeneratePrime(long min, long max)
        {
            Random random = new Random();
            long prime = min;
            while (PrimeTest(prime))
            {
                prime = random.NextInt64(min, max);
            }
            return prime;
        }
        public IActionResult GeneratePrimeNumbers()
        {
            Reset();
            Random random = new Random();
            p = GeneratePrime(1000, 10000);
            q = GeneratePrime(1000, 10000);
            n = p * q;
            o = (p - 1) * (q - 1);
            e = random.NextInt64(1, (long)n);
            while (NWD((long)o, (long)e) != 1)
                e = random.NextInt64(1, (long)n);
            d = RozszerzonyAlgorymEuklidesa();

            return View("Index");   
        }
        public static string getVarable(string x)
        {
            switch (x)
            {
                case "p":
                    return p is null ? "null" : p.ToString();
                case "q":
                    return q is null ? "null" : q.ToString();
                case "o":
                    return o is null ? "null" : o.ToString();
                case "e":
                    return e is null ? "null" : e.ToString();
                case "n":
                    return n is null ? "null" : n.ToString();
                case "d":
                    return d is null ? "null" : d.ToString();
                case "c":
                    return c is null ? "null" : c.ToString();
                case "t":
                    return t is null ? "null" : t.ToString();
                case "message":
                    return message is null ? "null" : message;
                case "messageHashed":
                    return messageHashed is null ? "null" : messageHashed;
                default:
                    return "error";
            }
        }
        public IActionResult Reset()
        {
            p = null;
            q = null;
            e = null;
            n = null;
            d = null;
            c = null;
            t = null;
            message = null;
            messageHashed = null;
            messageHashedArr = null;
            return View("Index");
        }
        private long NWD(long a, long b)
            => a == b ? a : a > b ? NWD(a - b, b) : NWD(a, b - a);

        private long? RozszerzonyAlgorymEuklidesa()
        {
            long u = 1;
            long x = 0;
            long w = (long)e;
            long z = (long)o;
            while (w != 0)
            {
                if (w < z)
                    (u, x, w, z) = (x, u, z, w);
                long l = w / z;
                u = u - (l * x);
                w = w - (l * z);
            }
            if (z != 1)
                return null;
            if (x < 0)
                x = x + (long)o;
            return x;
        }
        [HttpPost]
        public IActionResult zaszyfruj([FromForm] string text)
        {
            if (text is null || q is null)
                return View("Index");
            messageHashed = "";
            messageHashedArr = new long[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                messageHashedArr[i] = pot_mod(text[i], (long)e, (long)n);
                messageHashed += messageHashedArr[i].ToString() + " ";
            }
            messageHashed = messageHashed.TrimEnd();
            return View("Index");
        }
        public IActionResult rozszyfruj([FromForm] string text)
        {
            if (text is null || q is null)
                return View("Index");
            message = "";
            long[] arr = Array.ConvertAll<string, long>(text.Split(" "), long.Parse);
            for (int i = 0; i < arr.Length; i++)
            {
                long aaa = pot_mod(7, 103, 143);
                long bbb = pot_mod(aaa, (long)d, (long)n);
                message += (char)pot_mod(arr[i], (long)d, (long)n);
            }
            return View("Index");
        }
        long pot_mod(long a, long w, long n)
        {
            long pot, wyn, q;

            pot = a; wyn = 1;
            for (q = w; q > 0; q /= 2)
            {
                if (q % 2 != 0) wyn = (wyn * pot) % n;
                pot = (pot * pot) % n;
            }
            return wyn;
        }

    }
}