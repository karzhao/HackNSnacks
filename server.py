import http.server
import socketserver
import os

# Set the directory where your Unity WebGL build is located
BUILD_DIR = "Build"

# Change the working directory to the build directory
os.chdir(BUILD_DIR)

# Define the port you want to serve on
PORT = 8001

class CustomHTTPRequestHandler(http.server.SimpleHTTPRequestHandler):
    def end_headers(self):
        # Add custom headers for Brotli files
        if self.path.endswith('.br'):
            self.send_header('Content-Encoding', 'br')
        super().end_headers()

# Create the server
with socketserver.TCPServer(("", PORT), CustomHTTPRequestHandler) as httpd:
    print(f"Serving at port {PORT}")
    httpd.serve_forever()