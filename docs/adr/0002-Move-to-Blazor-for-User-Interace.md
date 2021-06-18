# Move to Blazor for User Interface

* Status: accepted
* Deciders: Timm Hoffmeister
* Date: 2021-06-18

Technical Story: Blazor is a framework to create web UIs within the .NET C# eco-system. 

## Context and Problem Statement

Currently, there's a separate build pipeline for the Web UI of Bhasha. Ideally, both, front- and back-end, can be built and deployed within the same step in github. Goal of this ADR is to suggest a way to reduce the effort for new developers to setup and run the entire Bhasha system on their local computer.

## Considered Options

* Integrate NPM build into Visual Studio as a post-build step
* Move UI to Blazor which integrates well with Visual Studio

## Decision Outcome

Even though the first option - to integrate NPM build into the VS project as a post-build step - is fairly cheap in regards to effort, the second option provides multiple advantages.

Pro Blazor:
1. reduces dependencies required to build the project (NPM)
2. entire code-base is written in C#/HTML/CSS (no JS knowledge required to join project)
3. out-of-the-box integration with Visual Studio

Cons Blazor:
1. react is more widely used & currently better supported
2. effort to re-write the current UI
