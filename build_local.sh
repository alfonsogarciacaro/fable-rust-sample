#!/bin/bash

echo Using local version of Fable
echo Make sure Fable repo is cloned to a sibling directory.
echo

pushd ../Fable
git checkout snake_island
git pull
popd
dotnet run -c release --project ../Fable/src/Fable.Cli -- watch --lang rust -e .rs --runWatch cargo run
