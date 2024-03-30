#!/bin/sh
# Start .NET app in the background
dotnet ServerlessAPI.dll &
# Start Node.js app in the background
cd MathMLApi && npm start