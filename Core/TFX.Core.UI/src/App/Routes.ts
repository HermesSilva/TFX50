
class Routes
{

    static Menu = "Menu/Lista";
    static AppModel = "Model/App";

}

class XAuthClient
{
    static AccessToken: string | null = null

    static async TryRestoreSession(): Promise<void>
    {
        const res = await fetch("/api/auth/refresh", {
            method: "POST",
            credentials: "include" // envia cookie com refreshToken
        })
        if (res.ok)
        {
            const json = await res.json()
            this.AccessToken = json.accessToken
        }
    }

    static async Login(email: string, password: string): Promise<void>
    {
        const res = await fetch("/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({
                email,
                password,
                userAgent: navigator.userAgent,
                IP: await this.ResolveIP() // via API externa opcional
            }),
            credentials: "include"
        })
        const json = await res.json()
        this.AccessToken = json.accessToken
    }

    static async Logout(): Promise<void>
    {
        await fetch("/api/auth/logout", {
            method: "POST",
            credentials: "include"
        })
        this.AccessToken = null
    }

    static async AuthorizedFetch(url: string, init: RequestInit = {}): Promise<Response>
    {
        if (!this.AccessToken) await this.TryRestoreSession()
        if (!this.AccessToken) throw new Error("Não autenticado")

        const res = await fetch(url, {
            ...init,
            headers: {
                ...init.headers,
                Authorization: `Bearer ${this.AccessToken}`
            }
        })

        if (res.status === 401)
        {
            await this.TryRestoreSession()
            return await this.AuthorizedFetch(url, init)
        }

        return res
    }

    static async ResolveIP(): Promise<string>
    {
        const res = await fetch("https://api.ipify.org?format=json")
        const json = await res.json()
        return json.ip
    }
}