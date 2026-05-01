import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { motion } from 'framer-motion'
import { Tabs, TabsContent, TabsList, TabsTrigger } from '@/components/ui/tabs'
import { Input } from '@/components/ui/input'
import { Label } from '@/components/ui/label'
import { CheckSquare, Loader2 } from 'lucide-react'
import api from '@/lib/api'
import { useAuthStore } from '@/stores/authStore'

export default function LoginPage() {
  const navigate = useNavigate()
  const login = useAuthStore(s => s.login)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  const [loginForm, setLoginForm] = useState({ email: '', password: '' })
  const [registerForm, setRegisterForm] = useState({ name: '', email: '', password: '' })

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError('')
    try {
      const { data } = await api.post('/api/auth/login', loginForm)
      const me = await api.get('/api/auth/me', {
        headers: { Authorization: `Bearer ${data.token}` },
      })
      login(data.token, me.data)
      navigate('/')
    } catch {
      setError('Invalid email or password.')
    } finally {
      setLoading(false)
    }
  }

  const handleRegister = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    setError('')
    try {
      await api.post('/api/auth/register', registerForm)
      const { data } = await api.post('/api/auth/login', {
        email: registerForm.email,
        password: registerForm.password,
      })
      const me = await api.get('/api/auth/me', {
        headers: { Authorization: `Bearer ${data.token}` },
      })
      login(data.token, me.data)
      navigate('/')
    } catch {
      setError('Registration failed. Email may already be in use.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="min-h-dvh flex items-center justify-center px-4 relative overflow-hidden">
      {/* Background glow */}
      <div className="absolute inset-0 flex items-center justify-center pointer-events-none">
        <div className="w-[600px] h-[600px] rounded-full bg-[#A855F7]/8 blur-[120px]" />
      </div>

      <motion.div
        initial={{ opacity: 0, y: 24 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.5, ease: [0.16, 1, 0.3, 1] }}
        className="w-full max-w-[420px] relative"
      >
        {/* Logo */}
        <div className="flex items-center gap-3 justify-center mb-8">
          <div className="w-10 h-10 rounded-xl bg-[#A855F7] flex items-center justify-center shadow-[0_0_30px_oklch(0.65_0.25_290/0.5)]">
            <CheckSquare className="w-5 h-5 text-white" />
          </div>
          <span className="font-heading font-800 text-xl tracking-tight text-white">TaskManager</span>
        </div>

        {/* Card */}
        <div className="glass rounded-2xl p-6">
          <Tabs defaultValue="login">
            <TabsList className="w-full mb-6 bg-white/[0.05] border border-white/[0.08] rounded-xl p-1">
              <TabsTrigger
                value="login"
                className="flex-1 rounded-lg text-sm data-[state=active]:bg-[#A855F7]/20 data-[state=active]:text-[#A855F7] data-[state=active]:shadow-none text-white/50 transition-all"
              >
                Sign in
              </TabsTrigger>
              <TabsTrigger
                value="register"
                className="flex-1 rounded-lg text-sm data-[state=active]:bg-[#A855F7]/20 data-[state=active]:text-[#A855F7] data-[state=active]:shadow-none text-white/50 transition-all"
              >
                Create account
              </TabsTrigger>
            </TabsList>

            {error && (
              <p className="text-sm text-red-400 mb-4 text-center bg-red-500/[0.08] rounded-lg py-2 px-3">
                {error}
              </p>
            )}

            {/* Login */}
            <TabsContent value="login">
              <form onSubmit={handleLogin} className="space-y-4">
                <div className="space-y-1.5">
                  <Label className="text-white/60 text-xs uppercase tracking-wider">Email</Label>
                  <Input
                    type="email"
                    required
                    placeholder="you@example.com"
                    value={loginForm.email}
                    onChange={e => setLoginForm(f => ({ ...f, email: e.target.value }))}
                    className="bg-white/[0.05] border-white/[0.10] text-white placeholder:text-white/25 focus-visible:ring-[#A855F7]/50 focus-visible:border-[#A855F7]/50"
                  />
                </div>
                <div className="space-y-1.5">
                  <Label className="text-white/60 text-xs uppercase tracking-wider">Password</Label>
                  <Input
                    type="password"
                    required
                    placeholder="••••••••"
                    value={loginForm.password}
                    onChange={e => setLoginForm(f => ({ ...f, password: e.target.value }))}
                    className="bg-white/[0.05] border-white/[0.10] text-white placeholder:text-white/25 focus-visible:ring-[#A855F7]/50 focus-visible:border-[#A855F7]/50"
                  />
                </div>
                <button
                  type="submit"
                  disabled={loading}
                  className="w-full h-10 rounded-lg bg-[#A855F7] text-white text-sm font-semibold hover:bg-[#EC4899] disabled:opacity-50 transition-all duration-200 flex items-center justify-center gap-2 shadow-[0_0_20px_oklch(0.65_0.25_290/0.3)] hover:shadow-[0_0_20px_oklch(0.62_0.22_335/0.4)]"
                >
                  {loading && <Loader2 className="w-4 h-4 animate-spin" />}
                  Sign in
                </button>
              </form>
            </TabsContent>

            {/* Register */}
            <TabsContent value="register">
              <form onSubmit={handleRegister} className="space-y-4">
                <div className="space-y-1.5">
                  <Label className="text-white/60 text-xs uppercase tracking-wider">Name</Label>
                  <Input
                    required
                    placeholder="Your name"
                    value={registerForm.name}
                    onChange={e => setRegisterForm(f => ({ ...f, name: e.target.value }))}
                    className="bg-white/[0.05] border-white/[0.10] text-white placeholder:text-white/25 focus-visible:ring-[#A855F7]/50 focus-visible:border-[#A855F7]/50"
                  />
                </div>
                <div className="space-y-1.5">
                  <Label className="text-white/60 text-xs uppercase tracking-wider">Email</Label>
                  <Input
                    type="email"
                    required
                    placeholder="you@example.com"
                    value={registerForm.email}
                    onChange={e => setRegisterForm(f => ({ ...f, email: e.target.value }))}
                    className="bg-white/[0.05] border-white/[0.10] text-white placeholder:text-white/25 focus-visible:ring-[#A855F7]/50 focus-visible:border-[#A855F7]/50"
                  />
                </div>
                <div className="space-y-1.5">
                  <Label className="text-white/60 text-xs uppercase tracking-wider">Password</Label>
                  <Input
                    type="password"
                    required
                    placeholder="••••••••"
                    value={registerForm.password}
                    onChange={e => setRegisterForm(f => ({ ...f, password: e.target.value }))}
                    className="bg-white/[0.05] border-white/[0.10] text-white placeholder:text-white/25 focus-visible:ring-[#A855F7]/50 focus-visible:border-[#A855F7]/50"
                  />
                </div>
                <button
                  type="submit"
                  disabled={loading}
                  className="w-full h-10 rounded-lg bg-[#A855F7] text-white text-sm font-semibold hover:bg-[#EC4899] disabled:opacity-50 transition-all duration-200 flex items-center justify-center gap-2 shadow-[0_0_20px_oklch(0.65_0.25_290/0.3)] hover:shadow-[0_0_20px_oklch(0.62_0.22_335/0.4)]"
                >
                  {loading && <Loader2 className="w-4 h-4 animate-spin" />}
                  Create account
                </button>
              </form>
            </TabsContent>
          </Tabs>
        </div>
      </motion.div>
    </div>
  )
}
