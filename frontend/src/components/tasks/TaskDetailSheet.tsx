import { useState, useEffect } from 'react'
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { Sheet, SheetContent, SheetHeader, SheetTitle } from '@/components/ui/sheet'
import { Loader2, Trash2, Send } from 'lucide-react'
import { formatDate } from '@/lib/utils'
import api from '@/lib/api'

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

interface Comment {
  id: string
  content: string
  userId: string
  createdAt: string
}

const statusOptions = ['ToDo', 'InProgress', 'Done', 'Canceled']
const priorityOptions = ['Low', 'Medium', 'High']

interface Props {
  task: Task | null
  projectId: string
  onClose: () => void
  onRefresh: () => void
}

export default function TaskDetailSheet({ task, projectId, onClose, onRefresh }: Props) {
  const queryClient = useQueryClient()
  const [comment, setComment] = useState('')

  const { data: comments = [], refetch: refetchComments } = useQuery<Comment[]>({
    queryKey: ['comments', task?.id],
    queryFn: async () => {
      const { data } = await api.get(`/api/tasks/${task!.id}/comments`)
      return data
    },
    enabled: !!task,
  })

  const addComment = useMutation({
    mutationFn: (content: string) =>
      api.post(`/api/tasks/${task!.id}/comments`, { content }),
    onSuccess: () => {
      setComment('')
      refetchComments()
    },
  })

  const deleteTask = useMutation({
    mutationFn: () =>
      api.delete(`/api/projects/${projectId}/tasks/${task!.id}`),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['tasks', projectId] })
      onRefresh()
      onClose()
    },
  })

  useEffect(() => {
    if (!task) setComment('')
  }, [task])

  return (
    <Sheet open={!!task} onOpenChange={open => !open && onClose()}>
      <SheetContent
        className="w-full sm:max-w-lg bg-[oklch(0.14_0.04_275)] border-white/[0.08] text-white overflow-y-auto"
        side="right"
      >
        {task && (
          <>
            <SheetHeader className="pb-4 border-b border-white/[0.08]">
              <SheetTitle className="font-heading font-600 text-lg text-white text-left pr-6">
                {task.title}
              </SheetTitle>
            </SheetHeader>

            <div className="py-5 space-y-5">
              {/* Meta */}
              <div className="grid grid-cols-2 gap-3">
                <div className="space-y-1.5">
                  <p className="text-xs text-white/40 uppercase tracking-wider">Status</p>
                  <select
                    defaultValue={task.status}
                    className="w-full h-8 px-2.5 rounded-lg bg-white/[0.05] border border-white/[0.10] text-sm text-white outline-none focus:border-[#A855F7]/50 transition-colors"
                  >
                    {statusOptions.map(s => <option key={s} value={s}>{s}</option>)}
                  </select>
                </div>
                <div className="space-y-1.5">
                  <p className="text-xs text-white/40 uppercase tracking-wider">Priority</p>
                  <select
                    defaultValue={task.priority}
                    className="w-full h-8 px-2.5 rounded-lg bg-white/[0.05] border border-white/[0.10] text-sm text-white outline-none focus:border-[#A855F7]/50 transition-colors"
                  >
                    {priorityOptions.map(p => <option key={p} value={p}>{p}</option>)}
                  </select>
                </div>
              </div>

              {task.dueDate && (
                <div className="space-y-1.5">
                  <p className="text-xs text-white/40 uppercase tracking-wider">Due date</p>
                  <p className="text-sm text-white/70">{formatDate(task.dueDate)}</p>
                </div>
              )}

              {task.description && (
                <div className="space-y-1.5">
                  <p className="text-xs text-white/40 uppercase tracking-wider">Description</p>
                  <p className="text-sm text-white/60 leading-relaxed">{task.description}</p>
                </div>
              )}

              {/* Comments */}
              <div className="space-y-3">
                <p className="text-xs text-white/40 uppercase tracking-wider">
                  Comments ({comments.length})
                </p>

                <div className="space-y-2 max-h-64 overflow-y-auto">
                  {comments.length === 0 ? (
                    <p className="text-sm text-white/25 py-4 text-center">No comments yet</p>
                  ) : (
                    comments.map(c => (
                      <div key={c.id} className="glass-subtle rounded-xl p-3">
                        <p className="text-sm text-white/80 leading-relaxed">{c.content}</p>
                        <p className="text-xs text-white/25 mt-1.5">{formatDate(c.createdAt)}</p>
                      </div>
                    ))
                  )}
                </div>

                {/* Add comment */}
                <div className="flex gap-2">
                  <input
                    value={comment}
                    onChange={e => setComment(e.target.value)}
                    onKeyDown={e => e.key === 'Enter' && !e.shiftKey && comment && addComment.mutate(comment)}
                    placeholder="Write a comment..."
                    className="flex-1 h-9 px-3 rounded-lg bg-white/[0.05] border border-white/[0.10] text-sm text-white placeholder:text-white/25 outline-none focus:border-[#A855F7]/50 transition-colors"
                  />
                  <button
                    onClick={() => comment && addComment.mutate(comment)}
                    disabled={!comment || addComment.isPending}
                    className="w-9 h-9 rounded-lg bg-[#A855F7]/20 border border-[#A855F7]/30 flex items-center justify-center text-[#A855F7] hover:bg-[#A855F7]/30 disabled:opacity-40 transition-all"
                  >
                    {addComment.isPending ? <Loader2 className="w-3.5 h-3.5 animate-spin" /> : <Send className="w-3.5 h-3.5" />}
                  </button>
                </div>
              </div>

              {/* Delete */}
              <div className="pt-2 border-t border-white/[0.06]">
                <button
                  onClick={() => deleteTask.mutate()}
                  disabled={deleteTask.isPending}
                  className="flex items-center gap-2 text-sm text-red-400/60 hover:text-red-400 hover:bg-red-400/[0.08] px-3 py-2 rounded-lg transition-all duration-150 w-full"
                >
                  {deleteTask.isPending ? <Loader2 className="w-4 h-4 animate-spin" /> : <Trash2 className="w-4 h-4" />}
                  Delete task
                </button>
              </div>
            </div>
          </>
        )}
      </SheetContent>
    </Sheet>
  )
}
