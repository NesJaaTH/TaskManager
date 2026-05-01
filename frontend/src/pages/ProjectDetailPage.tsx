import { useParams, useNavigate } from 'react-router-dom'
import { useQuery } from '@tanstack/react-query'
import { motion } from 'framer-motion'
import { ArrowLeft, Users, Loader2 } from 'lucide-react'
import api from '@/lib/api'
import KanbanBoard from '@/components/tasks/KanbanBoard'

interface Project {
  id: string
  name: string
  description: string
  status: string
  ownerId: string
}

interface Task {
  id: string
  title: string
  description: string
  status: string
  priority: string
  dueDate: string | null
  assigneeId: string | null
  createdById: string
}

const statusColors: Record<string, string> = {
  Active:   'text-emerald-400 bg-emerald-400/10',
  Archived: 'text-white/30 bg-white/[0.05]',
}

export default function ProjectDetailPage() {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()

  const { data: project, isLoading: loadingProject } = useQuery<Project>({
    queryKey: ['project', id],
    queryFn: async () => {
      const { data } = await api.get(`/api/projects/${id}`)
      return data
    },
  })

  const { data: tasks = [], isLoading: loadingTasks, refetch } = useQuery<Task[]>({
    queryKey: ['tasks', id],
    queryFn: async () => {
      const { data } = await api.get(`/api/projects/${id}/tasks`)
      return data
    },
    enabled: !!id,
  })

  if (loadingProject) {
    return (
      <div className="flex items-center justify-center py-32">
        <Loader2 className="w-6 h-6 animate-spin text-[#A855F7]" />
      </div>
    )
  }

  return (
    <div className="min-h-dvh flex flex-col">
      {/* Header */}
      <motion.div
        initial={{ opacity: 0, y: -8 }}
        animate={{ opacity: 1, y: 0 }}
        transition={{ duration: 0.3, ease: [0.16, 1, 0.3, 1] }}
        className="px-8 pt-8 pb-6 border-b border-white/[0.07]"
      >
        <button
          onClick={() => navigate('/')}
          className="flex items-center gap-2 text-sm text-white/30 hover:text-white/60 mb-4 transition-colors"
        >
          <ArrowLeft className="w-4 h-4" />
          Dashboard
        </button>

        <div className="flex items-start justify-between">
          <div>
            <div className="flex items-center gap-3 mb-1">
              <h1 className="font-heading font-700 text-2xl text-white tracking-tight">
                {project?.name}
              </h1>
              {project?.status && (
                <span className={`text-xs font-medium px-2 py-0.5 rounded-full ${statusColors[project.status] ?? ''}`}>
                  {project.status}
                </span>
              )}
            </div>
            {project?.description && (
              <p className="text-white/40 text-sm">{project.description}</p>
            )}
          </div>
          <div className="flex items-center gap-2 text-xs text-white/30">
            <Users className="w-3.5 h-3.5" />
            Members
          </div>
        </div>
      </motion.div>

      {/* Kanban */}
      <div className="flex-1 pt-6 overflow-hidden">
        {loadingTasks ? (
          <div className="flex items-center justify-center py-24">
            <Loader2 className="w-5 h-5 animate-spin text-[#A855F7]" />
          </div>
        ) : (
          <KanbanBoard tasks={tasks} projectId={id!} onRefresh={refetch} />
        )}
      </div>
    </div>
  )
}
