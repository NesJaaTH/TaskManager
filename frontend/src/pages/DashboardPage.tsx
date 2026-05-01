import { useState } from 'react'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { useNavigate } from 'react-router-dom'
import { motion } from 'framer-motion'
import { Plus, FolderKanban, Users, CheckSquare, MoreHorizontal, Loader2 } from 'lucide-react'
import { useAuthStore } from '@/stores/authStore'
import api from '@/lib/api'
import { formatDate } from '@/lib/utils'

interface Project {
  id: string
  name: string
  description: string
  status: string
  createdAt: string
  updatedAt: string
}

const statusColors: Record<string, string> = {
  Active: 'text-emerald-400 bg-emerald-400/10',
  Archived: 'text-white/30 bg-white/[0.05]',
}

export default function DashboardPage() {
  const { user } = useAuthStore()
  const navigate = useNavigate()
  const queryClient = useQueryClient()
  const [showCreate, setShowCreate] = useState(false)
  const [newProject, setNewProject] = useState({ name: '', description: '' })

  const hour = new Date().getHours()
  const greeting = hour < 12 ? 'Good morning' : hour < 18 ? 'Good afternoon' : 'Good evening'

  const { data: projects = [], isLoading } = useQuery<Project[]>({
    queryKey: ['projects'],
    queryFn: async () => {
      const { data } = await api.get('/api/projects')
      return data
    },
  })

  const createMutation = useMutation({
    mutationFn: (body: { name: string; description: string }) =>
      api.post('/api/projects', body),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['projects'] })
      setShowCreate(false)
      setNewProject({ name: '', description: '' })
    },
  })

  return (
    <div className="p-8 max-w-6xl mx-auto">
      {/* Header */}
      <motion.div
        initial={{ opacity: 0, y: 16 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.4, ease: [0.16, 1, 0.3, 1] }}
        className="flex items-start justify-between mb-10"
      >
        <div>
          <p className="text-white/40 text-sm mb-1">{greeting}</p>
          <h1 className="font-heading font-700 text-3xl text-white tracking-tight">
            {user?.name ?? 'Welcome back'}
          </h1>
        </div>
        <button
          onClick={() => setShowCreate(true)}
          className="flex items-center gap-2 px-4 h-10 rounded-xl bg-[#A855F7] text-white text-sm font-semibold hover:bg-[#EC4899] transition-all duration-200 shadow-[0_0_20px_oklch(0.65_0.25_290/0.3)] hover:shadow-[0_0_20px_oklch(0.62_0.22_335/0.4)]"
        >
          <Plus className="w-4 h-4" />
          New project
        </button>
      </motion.div>

      {/* Create project inline */}
      {showCreate && (
        <motion.div
          initial={{ opacity: 0, y: -8 }}
          animate={{ opacity: 1, y: 0 }}
          className="glass rounded-2xl p-5 mb-6"
        >
          <h2 className="font-heading font-600 text-base text-white mb-4">Create project</h2>
          <div className="space-y-3">
            <input
              autoFocus
              placeholder="Project name"
              value={newProject.name}
              onChange={e => setNewProject(p => ({ ...p, name: e.target.value }))}
              className="w-full h-10 px-3.5 rounded-lg bg-white/[0.05] border border-white/[0.10] text-white text-sm placeholder:text-white/25 outline-none focus:border-[#A855F7]/50 focus:ring-2 focus:ring-[#A855F7]/20 transition-all"
            />
            <input
              placeholder="Description (optional)"
              value={newProject.description}
              onChange={e => setNewProject(p => ({ ...p, description: e.target.value }))}
              className="w-full h-10 px-3.5 rounded-lg bg-white/[0.05] border border-white/[0.10] text-white text-sm placeholder:text-white/25 outline-none focus:border-[#A855F7]/50 focus:ring-2 focus:ring-[#A855F7]/20 transition-all"
            />
            <div className="flex gap-2">
              <button
                onClick={() => createMutation.mutate(newProject)}
                disabled={!newProject.name || createMutation.isPending}
                className="flex items-center gap-2 px-4 h-9 rounded-lg bg-[#A855F7] text-white text-sm font-medium hover:bg-[#EC4899] disabled:opacity-50 transition-all"
              >
                {createMutation.isPending && <Loader2 className="w-3.5 h-3.5 animate-spin" />}
                Create
              </button>
              <button
                onClick={() => setShowCreate(false)}
                className="px-4 h-9 rounded-lg text-white/40 text-sm hover:text-white/70 hover:bg-white/[0.05] transition-all"
              >
                Cancel
              </button>
            </div>
          </div>
        </motion.div>
      )}

      {/* Projects grid */}
      {isLoading ? (
        <div className="flex items-center justify-center py-24">
          <Loader2 className="w-6 h-6 animate-spin text-[#A855F7]" />
        </div>
      ) : projects.length === 0 ? (
        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          className="flex flex-col items-center justify-center py-24 text-center"
        >
          <div className="w-16 h-16 rounded-2xl bg-white/[0.04] border border-white/[0.07] flex items-center justify-center mb-4">
            <FolderKanban className="w-7 h-7 text-white/20" />
          </div>
          <p className="text-white/50 font-medium mb-1">No projects yet</p>
          <p className="text-white/25 text-sm">Create your first project to get started</p>
        </motion.div>
      ) : (
        <motion.div
          initial={{ opacity: 0 }}
          animate={{ opacity: 1 }}
          transition={{ delay: 0.1 }}
          className="grid gap-4"
          style={{ gridTemplateColumns: 'repeat(auto-fill, minmax(280px, 1fr))' }}
        >
          {projects.map((project, i) => (
            <motion.div
              key={project.id}
              initial={{ opacity: 0, y: 16 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ delay: i * 0.05, ease: [0.16, 1, 0.3, 1] }}
              onClick={() => navigate(`/projects/${project.id}`)}
              className="glass rounded-2xl p-5 cursor-pointer hover:border-[#A855F7]/20 hover:bg-[#A855F7]/[0.03] transition-all duration-200 group"
            >
              <div className="flex items-start justify-between mb-3">
                <div className="w-10 h-10 rounded-xl bg-[#A855F7]/15 border border-[#A855F7]/20 flex items-center justify-center group-hover:bg-[#A855F7]/20 transition-colors">
                  <FolderKanban className="w-4.5 h-4.5 text-[#A855F7]" />
                </div>
                <span className={`text-xs font-medium px-2 py-0.5 rounded-full ${statusColors[project.status] ?? 'text-white/40 bg-white/[0.05]'}`}>
                  {project.status}
                </span>
              </div>
              <h3 className="font-heading font-600 text-base text-white mb-1 line-clamp-1">{project.name}</h3>
              {project.description && (
                <p className="text-sm text-white/40 line-clamp-2 mb-3">{project.description}</p>
              )}
              <div className="flex items-center gap-3 text-xs text-white/25 mt-auto pt-3 border-t border-white/[0.06]">
                <span className="flex items-center gap-1">
                  <Users className="w-3 h-3" />
                  Members
                </span>
                <span className="flex items-center gap-1">
                  <CheckSquare className="w-3 h-3" />
                  Tasks
                </span>
                <span className="ml-auto">{formatDate(project.updatedAt)}</span>
              </div>
            </motion.div>
          ))}
        </motion.div>
      )}
    </div>
  )
}
