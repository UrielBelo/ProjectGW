import bpy
import json

def export_mesh():
    meshes = []
    
    for obj in bpy.context.scene.objects:
        if obj.type == 'MESH':
            mesh_data = {
                "name": obj.name,
                "vertices": [],
                "faces": []
            }
            
            mesh = obj.data
            obj_matrix = obj.matrix_world  # Transformação global do objeto
            
            # Exportar vértices transformados
            for vert in mesh.vertices:
                world_pos = obj_matrix @ vert.co  # Aplica transformação
                mesh_data["vertices"].append([world_pos.x, world_pos.y, world_pos.z])
            
            # Exportar faces (índices dos vértices)
            for face in mesh.polygons:
                mesh_data["faces"].append([vert_idx for vert_idx in face.vertices])
            
            meshes.append(mesh_data)
    
    # Salvar como JSON
    with open("terrain.json", "w") as f:
        json.dump(meshes, f, indent=2)

export_mesh()