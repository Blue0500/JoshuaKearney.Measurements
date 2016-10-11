using System;

namespace JoshuaKearney {

    public class Union<T, E> {
        private T first;
        private E second;
        private bool hasFirst;

        public Union(T data) {
            this.first = data;
            this.second = default(E);
            this.hasFirst = true;
        }

        public Union(E data) {
            this.first = default(T);
            this.second = data;
            this.hasFirst = false;
        }

        public void Match(Action<T> firstMatch, Action<E> secondMatch) {
            if (this.hasFirst) {
                firstMatch(this.first);
            }
            else {
                secondMatch(this.second);
            }
        }

        public TOut Select<TOut>(Func<T, TOut> firstMatch, Func<E, TOut> secondMatch) {
            if (this.hasFirst) {
                return firstMatch(this.first);
            }
            else {
                return secondMatch(this.second);
            }
        }

        public static implicit operator Union<T, E>(E that) {
            return new Union<T, E>(that);
        }

        public static implicit operator Union<T, E>(T that) {
            return new Union<T, E>(that);
        }

        public static implicit operator Union<E, T>(Union<T, E> that) {
            if (that.hasFirst) {
                return that.first;
            }
            else {
                return that.second;
            }
        }

        public static implicit operator Union<T, E>(Union<E, T> that) {
            if (that.hasFirst) {
                return that.first;
            }
            else {
                return that.second;
            }
        }
    }
}