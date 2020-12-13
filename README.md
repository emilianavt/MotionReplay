# MotionReplay

This is a very simple debugging tool for [VMC protocol](https://protocol.vmc.info/) applications with two functions:

* **Receive**: Receive data over VMC protocol on localhost over port 39539. When pressing save, the data received so far will be written to a JSON file and the buffered data will be cleared out.
* **Send**: This allows loading a packet dump produced by the receive function and playing it back. The data will be sent to 127.0.0.1 on port 39539. To make sure the timing is as accurate as possible, it uses busy waiting which will use up a whole CPU core, so don't use this if you don't have one core to spare.

Building this with Unity is a bit, but it worked for putting this together quickly and I could add a simple model preview later on.