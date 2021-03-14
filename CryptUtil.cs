using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Random = System.Random;

public class CryptUtil
{
     
    private static ulong P = 18446744073709551615;
    private static ulong G = 5;
    public static string Randomkey()
    {
        int n = 8;
        string _chars = "0123456789qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";
        Random r = new Random();
        char[] buffer = new char[n];
        for (int i = 0; i < n; i++)
        {
            buffer[i] = _chars[r.Next(_chars.Length)];
        }
        return new string(buffer);
    }
    
    public static string Dhexchange(string val)
    {
        if (val.Length != 8)
        {
            Debug.LogError(" Invalid dh key ");
            return null;
        }
        char[] x = val.ToCharArray();
        UInt32 []xx = new UInt32[2];
        xx[0] = (UInt32)(x[0] | x[1] << 8 | x[2] << 16 | x[3] << 24);
        xx[1] = (UInt32)(x[4] | x[5]<<8 | x[6]<<16 | x[7]<<24);
        ulong x64 =(ulong)(xx[0] | (ulong)xx[1] << 32);
        if (x64 == 0)
        {
            Debug.LogError(" Invalid dh key ");
            return null;
        }
        ulong r = powmodp(G,	x64);
        return Convert.ToString(r);
    }

    static ulong powmodp(ulong a, ulong b) {
        if (a > P)
            a %= P;
        return pow_mod_p(a,b);
    }
    
    static ulong pow_mod_p(ulong a, ulong b) {
        if (b == 1) {
            return a;
        }
        ulong t = pow_mod_p(a, b>>1);
        t = mul_mod_p(t,t);
        if (b % 2 > 0) {
            t = mul_mod_p(t, a);
        }
        return t;
    }
    
    static ulong mul_mod_p(ulong a, ulong b) {
        ulong m = 0;
        while (b > 0) {
            if ((b&1) > 0) {
                UInt64 t = P-a;
                if ( m >= t) {
                    m -= t;
                } else {
                    m += a;
                }
            }
            if (a >= P - a) {
                a = a * 2 - P;
            } else {
                a = a * 2;
            }
            b>>=1;
        }
        return m;
    }

}