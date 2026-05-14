import asyncio
import json
import websockets

HOST = "localhost"
PORT = 5000

async def handle(websocket):
    async for message in websocket:
        data = json.loads(message)
        if isinstance(data, list) and len(data) == 2:
            string1, string2 = data[0], data[1]
            print(f"Received: {string1}, {string2}")
            # handle the strings here

async def main():
    print(f"WebSocket server listening on ws://{HOST}:{PORT}")
    async with websockets.serve(handle, HOST, PORT):
        await asyncio.Future()  # run forever

if __name__ == "__main__":
    asyncio.run(main())
