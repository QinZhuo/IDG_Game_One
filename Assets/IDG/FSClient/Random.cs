using System.Net.Sockets;
using System;
using System.Linq;
namespace IDG.FSClient{
    public class Random
    {
        ulong seed;
        public Random(ulong seed){
            this.seed=seed;
        }

        public int next(){
           seed=seed*214013L;
           return (int)((seed>>16)&0x7fff);
        }

        public int Range(int max){
            return next()%max;
        }
    }
}