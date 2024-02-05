# Arcadia

Simple-minded barebone websocket-based message server where anyone is welcome. All users are welcomed as a unique guest untill lock-in a login. Arcadia doesn't store chat history and uses a very simple token-based authentication to provide basic identity. The purpose of this server is: 1) Provide a live chatting environment for Parcel, 2) Foster primitive communities, 3) Make it super easy to maintain and super easy to develop a client for.

Simple message format: `-channel message`. Where message may be used as a command.  
Channel should not contain space and must start with `-`; If no channel is specified, it uses default channel.

(At the moment Arcadia doesn't have a central server yet)

## Commands

Commands can be used in message. Commands are case-insensitive.

* (Default) `!speak <content>`: Speak at specific channel.
* `!loging <username> <token> <email>`: Login to server; if username is not taken, it will be registered. Token is used to authenticate user and only the hash will be saved so it cannot be recovered - provide an email address if you wish to recover your account. Login provides benefits like unique username and server-side configuration storage (for listening channels). Login doesn't alter session ID (or guest ID); The same login may be used by multiple sessions/guest IDs.
* `!listen`: Listen to only specified channels - this should be saved as per user/login configuration.

## Developer Guide: Develop Custom Arcadia Client

To develop a custom Arcadia client, just connect to the server endpoint, then start messaging like you would on a chat!
