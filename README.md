# Arcadia

Simple-minded websocket message-based anyone-is-welcome server. All guests are welcomed as a unique guest untill login.

Simple message format: `-channel message`. Where message may be used as a command. Channel should not contain space and must start with `-`; If no channel is specified, it uses default channel.

## Commands

Commands can be used in message. Commands are case-insensitive.

* (Default) `!speak <content>`: Speak at specific channel.
* `!loging <username> <token> <email>`: Login to server; if username is not taken, it will be registered. Token is used to authenticate user and only the hash will be saved so it cannot be recovered - provide an email address if you wish to recover your account.
* `!listen`: Listen to only specified channels.