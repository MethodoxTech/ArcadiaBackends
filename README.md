# Arcadia

Simple-minded websocket message-based anyone-is-welcome server.

There are two kinds of messages, one is stateless, one is stated:

1. Stateless message includes all configurations in one shot: `{username:token} [channel] message`. Except `message`, all other arguments cannot contain space. This puts the user at the specified channel.
2. Stated messages use specific commands to do things.

## Commands

* `!set`: Set username.
* (Default) `!speak`: Speak at specific channel.
* `!listen`: Listen to only specified channels.