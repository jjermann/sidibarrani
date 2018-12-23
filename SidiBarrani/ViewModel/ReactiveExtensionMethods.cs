using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SidiBarrani.ViewModel
{
    public static class ReactiveExtensionMethods
    {
        public static IObservable<TResult> SelectLastAsync<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector)
        {
            var currentThreadScheduler = Scheduler.CurrentThread;
            var res = source
                .Select(x => Observable.FromAsync(cToken => Task.Factory.StartNew(
                    () => selector(x), 
                    cToken)
                ))
                .Switch()
                .ObserveOn(currentThreadScheduler);
            return res;
        }
        // public static IObservable<TResult> SelectLastAsync<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector)
        // {
        //     var currentThreadScheduler = Scheduler.CurrentThread;
        //     var res = source
        //         .Select(x => Observable.FromAsync(cToken => Task.Factory.StartNew(
        //             () => {
        //                 var task = Thread.CurrentThread;
        //                 using (cToken.Register(task.Abort))
        //                 {
        //                     return selector(x);
        //                 }
        //             }, 
        //             cToken)
        //         ))
        //         .Switch()
        //         .ObserveOn(currentThreadScheduler);
        //     return res;
        // }
    }
}