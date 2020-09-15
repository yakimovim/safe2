using System;

namespace Safe.Core.Domain
{
    public sealed class Password
    {
        private readonly string _password;

        public Password(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException(nameof(password));
            
            _password = password;
        }

        public override int GetHashCode() => _password.GetHashCode();

        public override bool Equals(object obj)
        {
            var another = obj as Password;

            if (another == null) return false;

            return _password == another._password;
        }

        public override string ToString() => _password;

        public static implicit operator string(Password password) => password._password;
    }
}
