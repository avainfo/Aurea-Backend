# 🎛️ Aurea-Backend — Music Extractor API

Backend service for the **Aurea** music app.
Provides a private, stateless API that resolves **audio-only** streams from YouTube links for in-app playback and (optionally) analytics and library features. Designed to remain **non-public**, protected by **Firebase Auth** and rate limiting.

---

## 🔗 Relationship to the Aurea App

**Aurea-Backend** is consumed exclusively by the **Aurea** client application.
It does not expose public endpoints and is intended for **private, educational use** only.

---

## ✨ What It Does (High-Level)

* **Extractor**

    * Takes a YouTube URL and returns the best audio-only stream (Opus/AAC) plus lightweight metadata
    * Short in-memory cache (≈10–15 min) to avoid repeated extraction of the same URL

* **Redirect / Proxy (Optional)**

    * **Redirect**: 302 to the current audio stream for simple player integrations
    * **Proxy**: backend can relay the audio stream (supports range requests) when stricter control is required

* **Search & Metadata (Optional)**

    * Minimal `/search?q=` to relay safe YouTube search results (server-side keys and quotas remain private)

* **User Data & Analytics (Optional)**

    * **Playlists / Favorites**: CRUD endpoints if you choose to centralize server-side instead of using Firestore directly
    * **Listening Events**: accept batched pings to enable a yearly **Wrapped** (aggregations run offline or on a schedule)

---

## 🧭 Endpoints (Conceptual Overview)

* `GET /health` — Health status.
* `GET /extract?url=` — Resolve audio-only stream + minimal metadata
* `GET /redirect?url=` — 302 redirect to the resolved audio stream
* `GET /proxy?url=` — Optional relay of the audio stream (bandwidth-heavier)
* `GET /search?q=` — Minimal search relay
* Playlists / Library / Listening-events endpoints for centralized data

> Payload shapes are intentionally omitted here; this repository documents **capabilities**, not usage.

---

## 🔐 Security & Access Control

* **Authentication**: Requires a valid **Firebase ID token** on every non-health request
* **Authorization**: Service intended for signed-in users of the Aurea app only
* **Rate Limiting**: Per user/IP throttling with burst control
* **CORS**: Restricted to app origins when relevant (e.g., web builds)
* **Input Validation**: Only accepts valid YouTube URLs
* **Privacy-First Logging**: Minimal structured logs; no sensitive tokens or personal data
* **No Long-Term Stream Storage**: Stream URLs are ephemeral and not persisted

---

## 🧱 Data Model (If Centralized Server-Side)

* **Tracks** — Minimal track records derived from YouTube (id, title, duration, thumbnail)
* **Playlists & Items** — User-owned collections referencing track IDs
* **Library (Favorites)** — Per-user liked/kept tracks
* **Listening Events** — Time-stamped play pings used for **Wrapped** analytics

*(If you prefer, the client can write to Firestore directly with strict Security Rules, keeping the backend stateless.)*

---

## 🤝 Contributors

* **Antonin Do Souto**
* **Hugo**
* **Pietro**

---

## 📜 License / Legal

This repository is for **educational, private use only**.
Do **not** expose the extractor publicly, and do **not** use it to distribute or monetize content in ways that violate platform **Terms of Service** or **copyright**.
