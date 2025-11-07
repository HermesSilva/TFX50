# 🔐 OWASP Security Audit — Cursor Prompt

You are a senior Application Security engineer. Audit this repository for security issues aligned with OWASP Top 10 (Web 2021) and OWASP API Security Top 10 (2023). Focus on .NET 8/9, Clean Architecture, Docker/Kubernetes, SQL Server, and CI/CD pipelines.

## REPO CONTEXT
- Monorepo: <describe the structure briefly if known, e.g., /src, /tests, /deploy, /infra>
- Tech: C#, .NET <version>, XUnit, NSubstitute, Docker, Kubernetes/Helm, SQL Server, Redis (if any), RabbitMQ/Kafka, GitHub/GitLab Actions.
- Entry points: <list API projects or folders>
- Threat model: Internet-exposed APIs; OAuth/OIDC (Okta/Auth0); API Keys.

## SCOPE
1) Code (C#, config, controllers, middleware, filters, validators).
2) API design (auth, authorization, input validation, rate limiting).
3) Secrets & configs (appsettings, env vars, CI, Helm/terraform/k8s manifests).
4) Dependencies & supply chain.
5) Dockerfiles & container runtime settings.
6) K8s manifests/Helm (RBAC, securityContext, network policies, ingress).
7) Logging/observability & privacy (PII).
8) SQL access patterns & migrations.

## WHAT TO CHECK (map each finding to at least one OWASP item)
- Injection/ORM misuse (SQL concat, FromSqlRaw with untrusted input, EF unsafe Includes).
- Broken Authentication/Authorization (missing [Authorize], overly broad policies, role checks in code, missing RequireHttps, JWT validation weaknesses, audience/issuer).
- Sensitive Data/Cryptographic Failures (plaintext secrets, weak crypto, no DPAPI/Azure KeyVault/Vault; HTTP not TLS).
- Insecure Design (missing rate limit/anti-automation, missing account lockout, predictable IDs, lack of idempotency).
- Security Misconfiguration (debug enabled, verbose errors, CORS *, missing HSTS, missing SameSite, path traversal, file upload validation).
- Vulnerable/Outdated Components (NuGet, npm, OS packages).
- Identification & Authentication Failures (API keys not scoped/rotated; JWT without kid pinning).
- SSRF, RCE, deserialization, template injection.
- API-specific (BOLA/IDOR, mass assignment, excessive data exposure, lack of pagination limits).
- Logging & Monitoring (lack of audit trails, PII in logs).
- Cloud/K8s (root containers, no readOnlyRootFilesystem, no dropCapabilities, permissive RBAC, hostPath, privileged, no NetworkPolicy).
- CI/CD (plaintext secrets, unpinned actions, missing least privilege).

IGNORE
- Auto-generated code, bin/obj, node_modules, .git, large binaries.

## DELIVERABLES
1) Executive summary (bullets): top 5 risks with business impact.
2) Findings table (Markdown) with columns:
   - ID | Severity (Critical/High/Medium/Low) | OWASP category | Component/Path | Evidence (snippet or file:line) | Exploit scenario | Fix recommendation.
3) Quick wins checklist (≤10 items) to apply in <2h>.
4) Secure diffs: propose minimal code patches for the top 3 High+ findings (show unified diffs).
5) Dependency risk report:
   - Run/approximate: `dotnet list package --vulnerable` and suggest upgrades with version ranges.
   - Flag unpinned GitHub/GitLab actions; propose pinned SHAs.
6) Runtime/container hardening recommendations:
   - Dockerfile: base image versioning, non-root user, HEALTHCHECK, no secret ARGs, distroless where possible.
   - K8s: `securityContext`, `readOnlyRootFilesystem`, `runAsNonRoot`, drop capabilities, resource limits, liveness/readiness, ingress TLS, `NetworkPolicy`, RBAC least privilege.
7) Secrets audit: report any hardcoded tokens/keys/passwords; suggest using Vault/KeyVault and env var patterns.

## STYLE
- Be concrete and opinionated. Include file paths and code excerpts with line numbers.
- Map each finding to OWASP: e.g., “A01:2021 – Broken Access Control”, “API1:2023 – BOLA”.
- Keep noise low; don’t flag theoretical issues without evidence.

## OPTIONAL ACTIONS (if applicable)
- Add middleware or filters for global validation and error handling.
- Enforce HTTPS redirection, HSTS, secure cookies, CORS allow-list.
- Add authorization policies for sensitive endpoints; replace role strings with constants/enums.
- Validate all `[FromBody]` DTOs with FluentValidation; reject unknown fields; limit payload size.
- Add rate limiting (ASP.NET RateLimiter) per IP and per client_id.
- Replace `FromSqlRaw` with parameterized queries; guard repositories.
- Sanitize logging (no PII, mask secrets).
- Add SAST rules (Semgrep) and secret scanning (trufflehog/git-secrets) configs.

## OUTPUT FORMAT
- One Markdown report with the sections above.
- Append a “Fix Pack” with ready-to-apply code diffs for the top issues.
- Keep recommendations aligned with .NET 8+ idioms and Clean Architecture boundaries.
