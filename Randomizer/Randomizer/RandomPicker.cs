using System;

namespace Randomizer
{
    public class RandomPicker<T>
    {
        public T Pick(T[] arr) 
        {
            var length = arr.Length;
            Random r = new Random();
            var i = r.Next(length - 1);        
            return arr[i];
        }        

    }
}