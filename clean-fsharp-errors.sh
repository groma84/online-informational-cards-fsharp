#!/bin/sh
DIRS="OnlineInformationalCards OnlineInformationalCards.Tests"
for dir in $DIRS; do
	rm -rf $dir/bin $dir/obj
done
for dir in $DIRS; do
	dotnet build $dir
done