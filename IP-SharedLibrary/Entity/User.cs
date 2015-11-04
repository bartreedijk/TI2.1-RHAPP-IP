﻿using System;

namespace IP_SharedLibrary.Entity
{
    public class User
    {
        public string Nickname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsDoctor { get; set; }

        public bool OnlineStatus { get; set; }

        public User(string nickname, string username, string password, bool isDoctor, bool onlineStatus)
        {
            Nickname = nickname;
            Username = username;
            Password = password;
            IsDoctor = isDoctor;
        }

        public User(string nickname, string username, string password, bool isDoctor)
        {
            Nickname = nickname;
            Username = username;
            Password = password;
            IsDoctor = isDoctor;
        }

        public User(string nickname, string username, string password)
        {
            Nickname = nickname;
            Username = username;
            Password = password;
        }

        public User()
        {

        }

        public void ChangeNickname(string nickname)
        {
            Nickname = nickname;
        }

        public void ChangePassword()
        {
            throw new NotImplementedException();
        }

        public void ClearPass()
        {
            this.Password = "";
        }

        public override string ToString()
        {
            return String.Format("{0} ({1})", Nickname, OnlineStatus ? "Online" : "Offline");
        }
    }
}