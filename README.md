Web Crawler

Overview

This project is a simple yet robust web crawler built using ASP.NET Core. 
It crawls a target URL, recursively discovers all internal links (same domain), and returns a list of unique pages in JSON format.


Tech Stack

- ASP.NET Core (.NET 8)
- Minimal API with layered architecture
- `HttpClient` for web requests
- `Uri` for safe and structured URL handling
- Regular Expressions (`Regex`) for HTML parsing
- Postman used for local testing


Architecture

This solution follows a clean, layered structure:

API Layer – Handles routing and request validation (`CrawlerController`)
Service Layer – Implements crawling logic (`CrawlerService`)
Core Interfaces & Models – Define contracts and response models

Uses Dependency Injection to keep layers loosely coupled.
