# C# Real-Time Chat Application
This is a real-time c# chat server I originally wrote while learning the language. Now I'm refining and improving it as a side project until it's no longer embarrasing. 

Right now, my primary focus is on the **server-side**, so the current client implementation is temporary and disgusting. A cleaner, more sophisticated Vue.js web client is further down the road. 

## **Implemented Features**
- Real-time messaging
- Basic Encryption
- Simple commands

## **Commands**
|  Command  | Description |
|-----------|-------------|
| `/who`    | List users currently connected |
| `/whisper {username}`| Send a private message to a user |

## **To-Do**
### **Core Improvements**
- Improve encryption to use randomly generated keys
- Add threading (Considering async/FIFO queue)

### **Client Replacement**
- Replace current client with a Vue.js web client
- Add controllers for web client interaction

### **Chat Enhancements**
- Add message colours
- Implement chat rooms
- Add permissions/roles system for rooms
- Expand the already exhaustive list of commands :)
